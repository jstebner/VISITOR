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
    private playerLineOfSight playerLineOfSightScript;
    private Visitor visitor;
    private int waypointIndex;
    private float distanceFromWaypoint;
    [SerializeField] private float minimumDistanceFromWaypoint;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float lookAngleBeforeMoving;
    private bool isUpstairs;
    private bool nextWaypointDirection = true; // true -> forward, false -> backward
    private int jumpsBeforeWaiting;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        visitor = GetComponent<Visitor>();
        jumpsBeforeWaiting = Random.Range(5, 15);
        playerLineOfSightScript = GameObject.Find("Player").GetComponent<playerLineOfSight>();
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
        if (nextWaypointDirection) {
            waypointIndex++;
            if (waypointIndex == activeWaypoints.Length) {
                waypointIndex = 0;
            }
        } else {
            waypointIndex--;
            if (waypointIndex == 0) {
                waypointIndex = activeWaypoints.Length - 1;
            }
        }
        jumpsBeforeWaiting--;
        Debug.Log(jumpsBeforeWaiting);
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
        jumpsBeforeWaiting--;
        distanceFromWaypoint = Vector2.Distance(transform.position, activeWaypoints[waypointIndex].position);     
    }

    public void setUpstairsActive() {
        if (activeWaypoints == null || !isUpstairs) {
            agent.SetDestination(agent.transform.position);
            agent.isStopped = true;
            activeWaypoints = upStairsWaypoints;
            if (activeWaypoints.Length == 0) {return;}
            do {
                waypointIndex = Random.Range(0, activeWaypoints.Length);
            } while (playerLineOfSightScript.hasLineOfSightWithWaypoint(activeWaypoints[waypointIndex]));
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
            agent.SetDestination(agent.transform.position);
            agent.isStopped = true;
            activeWaypoints = downStairsWaypoints;
            if (activeWaypoints.Length == 0) {return;}
            do {
                waypointIndex = Random.Range(0, activeWaypoints.Length);
            } while (playerLineOfSightScript.hasLineOfSightWithWaypoint(activeWaypoints[waypointIndex]));
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
        agent.SetDestination(agent.transform.position);
        agent.isStopped = true;
        activeWaypoints = null;
        setRemainingJumps(Random.Range(5, 15));
        bool newDirection = Random.Range(0, 2) == 1;
        Debug.Log($"Direction: {newDirection}");
        setDirection(newDirection);
    }

    private void setRemainingJumps(int jumps) {
        jumpsBeforeWaiting = jumps;
    }

    private void setDirection(bool direction) {
        nextWaypointDirection = direction;
    }

    public int getRemainingJumps() {
        return jumpsBeforeWaiting;
    }

}
