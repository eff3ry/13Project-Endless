using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button restartButton;


    private UiController uiController;
    void Awake()
    {
        uiController = FindObjectOfType<UiController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(delegate { Debug.Log("Restart"); restartGame(); });
    }


    void restartGame()
    {
        Time.timeScale = 1f;
        uiController.ChangeUiPanel(0);
        SceneManager.LoadScene("Game");
        
    }
    
}
