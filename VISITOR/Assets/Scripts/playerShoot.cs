using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{

    public static Action shootInput;
    public static Action reload;
    private bool holdingWeapon;
    public menuController menuControllerScript;

    void Start() {
        holdingWeapon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingWeapon || menuControllerScript.getPaused()) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            shootInput?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            reload?.Invoke();
        }
    }

    public void setHolding(bool hold) {
        holdingWeapon = hold;
    }
}
