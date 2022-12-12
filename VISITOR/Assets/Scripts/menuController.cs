using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public GameObject openingPrompt;
    [SerializeField] private float openingPromptTime;

    void Start() {
        StartCoroutine(closeOpeningPrompt());
    }

    public void backToMainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private IEnumerator closeOpeningPrompt() {
        yield return new WaitForSeconds(openingPromptTime);
        openingPrompt.SetActive(false);
    }
}
