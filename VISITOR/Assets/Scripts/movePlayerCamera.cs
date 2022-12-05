using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayerCamera : MonoBehaviour
{

    public Transform playerCameraPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCameraPosition.position;
    }
}
