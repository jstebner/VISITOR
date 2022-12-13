using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    private AudioSource playerAudioSource;
    private playerMovement playerMovementScript;

    void Start() {
        playerMovementScript = GetComponent<playerMovement>();
        playerAudioSource = GetComponentInChildren<AudioSource>();
    }

    void Update() {
        if (playerMovementScript.getGrounded() && GetComponent<Rigidbody>().velocity.magnitude > 0) {
            playerAudioSource.enabled = true;
        } else {
            playerAudioSource.enabled = false;
        }
    }



}
