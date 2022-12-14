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
    [SerializeField] private float minimumDistanceBeforeDeathTimer;

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

    public AudioSource visitorFootsteps;
    public AudioSource visitorChasingPlayer;
    public AudioSource playerDeathSound;

    private bool playerIsDead = false;
    private bool playerIsDownstairs; 

    private State state;
    public enum State {
        Patrol,
        TargetPlayer,
        Waiting,
    }

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        state = startingState;
        patrolScript = GetComponent<Patrol>();
        playerIsDownstairs = isPlayerDownstairs();
    }

    void FixedUpdate() {
        distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        switch (state) {
            case State.TargetPlayer:
                if (distanceFromPlayer <= minimumDistanceBeforeDeathTimer) {
                    closeToPlayerTime += Time.deltaTime;
                    resetTime = 0;
                    if (closeToPlayerTime >= maxCloseToPlayerTime && !playerIsDead) {
                        player.GetComponent<playerMovement>().canMove = false;
                        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        playerDeathSound.Play();
                        deathScreenUI.SetActive(true);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        playerCamera.setControl(false);
                        playerIsDead = true;
                        agent.SetDestination(agent.transform.position);
                        agent.isStopped = true;
                    }
                }
                if (distanceFromPlayer <= minimumDistanceFromPlayer) {
                    agent.SetDestination(agent.transform.position);
                    agent.isStopped = true;
                } else {
                    resetTime += Time.deltaTime;
                    if (resetTime >= maxResetTime) {
                        closeToPlayerTime = 0;
                    }
                    agent.isStopped = false;
                    trackPlayer();
                    timeTargetingPlayer += Time.deltaTime;
                    if (timeTargetingPlayer >= maxTimeToTargetPlayer) {
                        //Debug.Log("Visitor waiting because targeting player for too long");
                        setState(State.Waiting);
                        player.GetComponent<playerLineOfSight>().resetLookTime();
                    }
                }
                break;
            case State.Waiting:
                timeSpentWaiting += Time.deltaTime;
                if (timeSpentWaiting >= waitingTime) {
                    setState(State.Patrol);
                }
                break;
            case State.Patrol:
                if (playerIsDownstairs != isPlayerDownstairs() || patrolScript.getRemainingJumps() == 0) {
                    setState(State.Waiting);
                    playerIsDownstairs = isPlayerDownstairs();
                    return;
                }
                if (isPlayerDownstairs()) {
                    patrolScript.setDownStairsActive();
                }
                else {
                    patrolScript.setUpstairsActive();
                }
                break;
        }

        if (agent.velocity.magnitude > 0) {
            visitorFootsteps.enabled = true;
        } else {
            visitorFootsteps.enabled = false;
        }
    }

    public void Damage(float damage)
    {
        //Debug.Log("Damaged");
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
            player.GetComponent<playerMovement>().canMove = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        timeSpentWaiting = 0;
        closeToPlayerTime = 0;
        resetTime = 0;
        timeTargetingPlayer = 0;
        if (newState == State.TargetPlayer) {
            visitorChasingPlayer.enabled = true;
        } else {
            visitorChasingPlayer.enabled = false;
        }
        switch (newState) {
            case State.Waiting:
                //Debug.Log("Waiting");
                agent.Warp(waitingPosition.transform.position);
                waitingTime = Random.Range(5f,10f);
                agent.isStopped = true;
                state = State.Waiting;
                break;
            case State.Patrol:
                agent.speed = 3.5f;
                //Debug.Log("Patrol");
                agent.SetDestination(agent.transform.position);
                agent.isStopped = true;
                state = State.Patrol;
                patrolScript.resetActiveWaypoints();
                agent.isStopped = false;
                break;
            case State.TargetPlayer:
                agent.speed = 9f;
                //Debug.Log("TARGET PLAYER");
                agent.SetDestination(agent.transform.position);
                agent.isStopped = true;
                state = State.TargetPlayer;
                agent.isStopped = false;                
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
