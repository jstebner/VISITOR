using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public GameObject openingPrompt;
    [SerializeField] private float maxOpeningPromptTime; 
    private float openingPromptTime;

    public void backToMainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
        
    void Update() {
        openingPromptTime += Time.deltaTime;
        if (openingPromptTime >= maxOpeningPromptTime) {
            openingPrompt.SetActive(false);
        }
    }
}
