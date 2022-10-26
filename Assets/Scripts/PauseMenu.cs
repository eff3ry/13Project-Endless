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
        //assign methods to buttons
        resumeButton.onClick.AddListener(delegate {resume();});
        backButton.onClick.AddListener(delegate {back();});
    }


    void Update()
    {
        //check for esc kay and pause or unpause
        if (Input.GetKeyDown(KeyCode.Escape) && player.isAlive)
        {
            if (menu.activeSelf)
            {
                resume();
            }
            else
            {
                pause();
            }
            
        }
    }

    //freezes game and shows pause menu
    void pause()
    {
        Time.timeScale = 0;
        uiController.ChangeUiPanel(2);
    }
    //unfreezes and hides pause menu
    void resume()
    {
        Time.timeScale =1f;
        uiController.ChangeUiPanel(0);
    }

    //changes scene back to the main menu
    void back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Login");
    }
}
