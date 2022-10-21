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
        resumeButton.onClick.AddListener(delegate {resume();});
        backButton.onClick.AddListener(delegate {back();});
    }


    void Update()
    {
        //check for  esc kay and pause o unpause
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

    void pause()
    {
        Time.timeScale = 0;
        uiController.ChangeUiPanel(2);
    }
    void resume()
    {
        Time.timeScale =1f;
        uiController.ChangeUiPanel(0);
    }

    void back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Login");
    }
}
