using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour
{
    public Transform playerCamera;
    public GameObject gun;
    //public GameObject flashlight;
    [SerializeField] private float pickUpDistance;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInfo, pickUpDistance);
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.transform.tag == "Gun") {
                gun.SetActive(true);
                Destroy(hitInfo.transform.gameObject);
            }
            if (hitInfo.transform.tag == "Flashlight") {
                //flashlight.setActive(true);
            }
        }
        
    }
}
