using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class reticleScript : MonoBehaviour
{
    public Transform playerCamera;
    [SerializeField] private float interactDistance;
    public GameObject keybindIndicatorUI;
    public TextMeshProUGUI UItext;

    void Update() {
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInfo, interactDistance)) {
            setKeybindIndicator("");
            return;
        }
        switch (hitInfo.transform.tag) {
            case "keypad":
                setKeybindIndicator("F");
                break;
            case "Gun":
                setKeybindIndicator("E");
                break;
            default:
                setKeybindIndicator("");
                break;
        }
    }

    void setKeybindIndicator(string newIndicator) {
        UItext.text = newIndicator;
    }
}
