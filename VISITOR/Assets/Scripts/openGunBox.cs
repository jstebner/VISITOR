using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class openGunBox : MonoBehaviour
{
    private bool toOpen;
    [SerializeField] private float rotationSpeed;
    private Transform pivot;

    private string passcode = "";

    void Start() {
        pivot = GetComponent<Transform>();
        for (int i = 0; i < 4; i++) {
            passcode += Random.Range(0,10);
        }
        Debug.Log(passcode);
        GameObject.Find("code").GetComponent<TextMeshPro>().text = passcode;
    }

    void FixedUpdate() {
        if (toOpen && pivot.rotation.eulerAngles.x <= 45) {
            pivot.Rotate(new Vector3(1,0,0) * (rotationSpeed * Time.deltaTime));
        }
    }

    public void open() {
        toOpen = true;
    }

    public string getPasscode() {
        return passcode;
    }
}
