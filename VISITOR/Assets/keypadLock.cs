using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class keypadLock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private Transform Safe;
    public PlayerCamera playerCamera;
    private GameObject keypadCanvas;
    private string correctCode = "1234";
    private string currentCodeValue = "";

    void OnEnable() {
        Debug.Log("camera control: false");
        playerCamera.setControl(false);
    }

    void OnDisable() {
        playerCamera.setControl(true);
        Debug.Log("camera control: true");
    }

    void Start() {
        keypadCanvas = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        codeText.text = currentCodeValue;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            currentCodeValue = "";
            keypadCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void openSafe() {
        Debug.Log("Opened");
    }

    public void addDigit(string digit) {
        if (currentCodeValue.Length < 4) {
            currentCodeValue += digit;
        }
    }

    public void confirmCode() {
        if (currentCodeValue == correctCode) {
            openSafe();
            currentCodeValue = "";
            keypadCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            currentCodeValue = "";
        } 
    }

    public void clearCode() {
        currentCodeValue = "";
    }
}
