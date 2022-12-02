using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool loop;
    private bool patrol = true;
    private NavMeshAgent agent;
    private int waypointIndex;
    private float distanceFromWaypoint;
    [SerializeField] private float minimumDistanceFromWaypoint;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float lookAngleBeforeMoving;
    private bool nextWaypointDirection = true; // true -> forward, false -> backward
    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        agent = this.GetComponent<NavMeshAgent>();
        if (waypoints.Length == 0)
            patrol = false; 
        
        if (patrol)
            transform.LookAt(waypoints[waypointIndex]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!patrol) {return;}
        Vector3 currentPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 waypointPosition = new Vector3(waypoints[waypointIndex].position.x, 0f, waypoints[waypointIndex].position.z);
        distanceFromWaypoint = Vector3.Distance(currentPosition, waypointPosition);
        if (distanceFromWaypoint <= minimumDistanceFromWaypoint) {
            if (loop) {
                IncrementLoopWaypointIndex();
            }
            else {
                IncrementBackAndForthWaypointIndex();
            }
        }
        var targetRotation = Quaternion.LookRotation(waypoints[waypointIndex].position - transform.position);
        if (Quaternion.Angle(transform.rotation, targetRotation) > lookAngleBeforeMoving) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            return;
        }
        agent.SetDestination(waypoints[waypointIndex].position);
    }

    void startPatrol() {
        patrol = true;
    }

    void stopPatrol() {
        patrol = false;
    }

    void IncrementLoopWaypointIndex() {
        waypointIndex++;
        if (waypointIndex == waypoints.Length) {
            waypointIndex = 0;
        }
        distanceFromWaypoint = Vector2.Distance(transform.position, waypoints[waypointIndex].position);
    }

    void IncrementBackAndForthWaypointIndex() {
        if (waypointIndex == 0 || waypointIndex == waypoints.Length - 1) {
            nextWaypointDirection = !nextWaypointDirection;
        }
        if (nextWaypointDirection && waypointIndex < waypoints.Length - 1) {
            waypointIndex++;
        } else if (!nextWaypointDirection && waypointIndex > 0) {
            waypointIndex--;
        }
        distanceFromWaypoint = Vector2.Distance(transform.position, waypoints[waypointIndex].position);     
    }
}
