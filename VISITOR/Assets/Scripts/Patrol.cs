using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform[] upStairsWaypoints;
    [SerializeField] private Transform[] downStairsWaypoints;
    private Transform[] activeWaypoints;
    [SerializeField] private bool loop;
    private bool patrol = false;
    private NavMeshAgent agent;
    private Visitor visitor;
    private int waypointIndex;
    private float distanceFromWaypoint;
    [SerializeField] private float minimumDistanceFromWaypoint;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float lookAngleBeforeMoving;
    private bool isUpstairs;
    private bool nextWaypointDirection = true; // true -> forward, false -> backward
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        visitor = GetComponent<Visitor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        patrol = visitor.getState() == Visitor.State.Patrol && activeWaypoints?.Length > 0;
        if (!patrol) {return;}
        Vector3 currentPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 waypointPosition = new Vector3(activeWaypoints[waypointIndex].position.x, 0f, activeWaypoints[waypointIndex].position.z);
        distanceFromWaypoint = Vector3.Distance(currentPosition, waypointPosition);
        if (distanceFromWaypoint <= minimumDistanceFromWaypoint) {
            if (loop) {
                IncrementLoopWaypointIndex();
            }
            else {
                IncrementBackAndForthWaypointIndex();
            }
        }
        var targetRotation = Quaternion.LookRotation(activeWaypoints[waypointIndex].position - transform.position);
        if (Quaternion.Angle(transform.rotation, targetRotation) > lookAngleBeforeMoving) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            return;
        }
        agent.SetDestination(activeWaypoints[waypointIndex].position);
    }

    void IncrementLoopWaypointIndex() {
        waypointIndex++;
        if (waypointIndex == activeWaypoints.Length) {
            waypointIndex = 0;
        }
        distanceFromWaypoint = Vector2.Distance(transform.position, activeWaypoints[waypointIndex].position);
    }

    void IncrementBackAndForthWaypointIndex() {
        if (waypointIndex == 0 || waypointIndex == activeWaypoints.Length - 1) {
            nextWaypointDirection = !nextWaypointDirection;
        }
        if (nextWaypointDirection && waypointIndex < activeWaypoints.Length - 1) {
            waypointIndex++;
        } else if (!nextWaypointDirection && waypointIndex > 0) {
            waypointIndex--;
        }
        distanceFromWaypoint = Vector2.Distance(transform.position, activeWaypoints[waypointIndex].position);     
    }

    public void setUpstairsActive() {
        if (activeWaypoints == null || !isUpstairs) {
            agent.isStopped = true;
            activeWaypoints = upStairsWaypoints;
            if (activeWaypoints.Length == 0) {return;}
            waypointIndex = Random.Range(0, activeWaypoints.Length);
            agent.Warp(activeWaypoints[waypointIndex].position);
            if (loop) {
                IncrementLoopWaypointIndex();
            }
            else {
                IncrementBackAndForthWaypointIndex();
            }
            agent.isStopped = false;
            agent.SetDestination(activeWaypoints[waypointIndex].position);
            isUpstairs = true;
        }
    }

    public void setDownStairsActive() {
        if (activeWaypoints == null || isUpstairs) {
            agent.isStopped = true;
            activeWaypoints = downStairsWaypoints;
            if (activeWaypoints.Length == 0) {return;}
            waypointIndex = Random.Range(0, activeWaypoints.Length);
            agent.Warp(activeWaypoints[waypointIndex].position);
            if (loop) {
                IncrementLoopWaypointIndex();
            }
            else {
                IncrementBackAndForthWaypointIndex();
            }
            agent.isStopped = false;
            agent.SetDestination(activeWaypoints[waypointIndex].position);
            isUpstairs = false;
        }
    }

    public void resetActiveWaypoints() {
        agent.isStopped = true;
        activeWaypoints = null;
    }
}
