using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour
{
    public GameObject keypad;
    public Transform playerCamera;
    //public GameObject flashlight;
    [SerializeField] private float interactDistance;
    private Transform physicalKeypadObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInfo, interactDistance);
            if (hitInfo.transform?.tag == "keypad") {
                physicalKeypadObject = hitInfo.transform;
                keypad.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Keypad");
            }
        }
        if (keypad.activeSelf == true) {
            if (Vector3.Distance(playerCamera.position, physicalKeypadObject.position) > interactDistance) {
                keypad.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
