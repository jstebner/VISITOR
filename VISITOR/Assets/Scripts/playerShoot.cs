using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{

    public static Action shootInput;
    public static Action reload;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            shootInput?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            reload?.Invoke();
        }
    }
}
