using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 200f;
    private NavMeshAgent agent;
    private Patrol patrolScript;
    public Transform player;
    private float distanceFromPlayer;
    [SerializeField] private float minimumDistanceFromPlayer;

    [SerializeField] private float maxCloseToPlayerTime;
    private float closeToPlayerTime;
    [SerializeField] private float maxResetTime;
    private float resetTime;
    [SerializeField] private float maxTimeToTargetPlayer;
    private float timeTargetingPlayer;

    private float waitingTime;
    private float timeSpentWaiting;

    [SerializeField] private State startingState;
    [SerializeField] private bool canSwitchState;

    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject winScreenUI;

    [SerializeField] private GameObject waitingPosition;
    
    public PlayerCamera playerCamera;

    private State state;
    public enum State {
        Patrol,
        NonPhysical,
        TargetPlayer,
        Waiting,
    }

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        state = startingState;
        patrolScript = GetComponent<Patrol>();
    }

    void Update() {
        distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        switch (state) {
            case State.TargetPlayer:
                if (distanceFromPlayer <= minimumDistanceFromPlayer) {
                    agent.isStopped = true;
                    closeToPlayerTime += Time.deltaTime;
                    resetTime = 0;
                    Debug.Log(closeToPlayerTime);
                    if (closeToPlayerTime >= maxCloseToPlayerTime) {
                        player.GetComponent<playerMovement>().canMove = false;
                        deathScreenUI.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        playerCamera.setControl(false);
                    }
                } else {
                    resetTime += Time.deltaTime;
                    if (resetTime >= maxResetTime) {
                        closeToPlayerTime = 0;
                    }
                    agent.isStopped = false;
                    trackPlayer();
                }
                timeTargetingPlayer += Time.deltaTime;
                if (timeTargetingPlayer >= maxTimeToTargetPlayer) {
                    setState(State.Waiting);
                    timeTargetingPlayer = 0;
                    player.GetComponent<playerLineOfSight>().resetLookTime();
                }
                break;
            case State.Waiting:
                timeSpentWaiting += Time.deltaTime;
                if (timeSpentWaiting >= waitingTime) {
                    State newState;
                    newState = (State)Random.Range(0,1); // Randomly choose between patrol or nonphysical
                    timeSpentWaiting = 0;
                    setState(newState);
                }
                break;
            case State.NonPhysical:
                timeSpentWaiting += Time.deltaTime;
                if (timeSpentWaiting >= waitingTime) {
                    setState(State.Waiting);
                }
                break;
            case State.Patrol:
                if (isPlayerDownstairs()) {
                    patrolScript.setDownStairsActive();
                }
                else {
                    patrolScript.setUpstairsActive();
                }
                break;
        }
    }

    public void Damage(float damage)
    {
        Debug.Log("Damaged");
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
            player.GetComponent<playerMovement>().canMove = false;
            winScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerCamera.setControl(false);
        }
    }

    private void trackPlayer() {
        agent.SetDestination(player.position);
    }

    public void setState(State newState) {
        if (!canSwitchState) return;

        switch (newState) {
            case State.NonPhysical:
                Debug.Log("Nonphysical");
                waitingTime = Random.Range(5f,10f);
                agent.isStopped = true;
                state = State.NonPhysical;
                break;
            case State.Waiting:
                Debug.Log("Waiting");
                agent.Warp(waitingPosition.transform.position);
                waitingTime = Random.Range(5f,10f);
                agent.isStopped = true;
                state = State.Waiting;
                break;
            case State.Patrol:
                Debug.Log("Patrol");
                agent.isStopped = true;
                state = State.Patrol;
                patrolScript.resetActiveWaypoints();
                agent.isStopped = false;
                break;
            case State.TargetPlayer:
                Debug.Log("TARGET PLAYER");
                agent.isStopped = true;
                state = State.TargetPlayer;
                agent.isStopped = false;                
                break;
            default:
                Debug.Log("Invalid State setting to waiting...");
                Debug.Log("Waiting");
                waitingTime = Random.Range(5f,10f);
                agent.isStopped = true;
                state = State.Waiting;
                break;
        }
    }

    public State getState() {
        return state;
    }

    private bool isPlayerDownstairs() {
        return player.position.y < 4f;
    }
}
