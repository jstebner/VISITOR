using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openGunBox : MonoBehaviour
{
    private bool toOpen;
    [SerializeField] private float rotationSpeed;
    private Transform pivot;

    void Start() {
        pivot = GetComponent<Transform>();
    }

    void Update() {
        if (toOpen && pivot.rotation.eulerAngles.x <= 45) {
            pivot.Rotate(new Vector3(1,0,0) * (rotationSpeed * Time.deltaTime));
        }
    }

    public void open() {
        toOpen = true;
    }
}
