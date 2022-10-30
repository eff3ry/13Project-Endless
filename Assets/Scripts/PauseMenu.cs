using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Button resumeButton;
    [SerializeField] Button backButton;
    [SerializeField] UiController uiController;
    public Player player;

    void Start()
    {
        // Assign methods to buttons
        resumeButton.onClick.AddListener(delegate {resume();});
        backButton.onClick.AddListener(delegate {back();});
    }


    void Update()
    {
        // Check for esc key and pause or unpause
        if (Input.GetKeyDown(KeyCode.Escape) && player.isAlive)
        {
            // If active call resume and if not active call pause
            (menu.activeSelf)? resume() : pause();            
        }
    }

    // Freezes game and shows pause menu
    void pause()
    {
        Time.timeScale = 0;
        uiController.ChangeUiPanel(2);
    }
    // Unfreezes and hides pause menu
    void resume()
    {
        Time.timeScale =1f;
        uiController.ChangeUiPanel(0);
    }

    // Changes scene back to the main menu
    void back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Login");
    }
}
