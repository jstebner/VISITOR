using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public GameObject openingPrompt;
    [SerializeField] private float openingPromptTime;

    public GameObject KeypadUI;
    public GameObject DeathScreenUI;
    public GameObject WinScreenUI;
    public GameObject PauseScreenUI;

    private bool isPaused = false;

    void Start() {
        PauseScreenUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;
        openingPrompt.SetActive(true);
        StartCoroutine(closeOpeningPrompt());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log($"Keypad: {KeypadUI.activeSelf}");
            Debug.Log($"DeathScreen: {DeathScreenUI.activeSelf}");
            Debug.Log($"WinScreen: {WinScreenUI.activeSelf}");
            Debug.Log($"Paused: {isPaused}");
            if (isPaused) {
                PauseScreenUI.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                AudioListener.pause = false;
                return;
            } 

            if (!KeypadUI.activeSelf && !DeathScreenUI.activeSelf && !WinScreenUI.activeSelf) {
                PauseScreenUI.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                AudioListener.pause = true;
            }
        }
    }

    public void backToMainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private IEnumerator closeOpeningPrompt() {
        yield return new WaitForSeconds(openingPromptTime);
        openingPrompt.SetActive(false);
    }

    public bool getPaused() {
        return isPaused;
    }
}
