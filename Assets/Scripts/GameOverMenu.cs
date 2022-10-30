using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button saveScoreButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private Player player;

    private UserManager userManager;
    private UiController uiController;
    void Awake()
    {
        // Find helpful objects
        uiController = FindObjectOfType<UiController>();
        userManager = FindObjectOfType<UserManager>();
    }

    void Start()
    {
        // Set the methods that the buttons will call
        restartButton.onClick.AddListener(delegate { Debug.Log("Restart"); restartGame(); });
        saveScoreButton.onClick.AddListener(delegate {saveHighScore(player.score); });
        backToMenuButton.onClick.AddListener(delegate {backToMenu();});
    }

    void OnEnable()
    {
        // Show save score button only if score > highscore
        saveScoreButton.gameObject.SetActive(player.score > userManager.currentUser.highScore);
        saveScoreButton.interactable = true;

        // Update Scores when this object is loaded
        highscoreText.text = $"Highscore: {userManager.currentUser.highScore}";
        scoreText.text = $"Score: {player.score}";
    }

    // Saves current score to highscore
    void saveHighScore(int score)
    {
        userManager.lastLoadedData[userManager.currentUserIndex].highScore = score;
        userManager.currentUser = userManager.lastLoadedData[userManager.currentUserIndex];
        userManager.saveData(userManager.lastLoadedData);
        saveScoreButton.interactable = false;
        highscoreText.text = $"Highscore: {userManager.currentUser.highScore}";
    }

    // Change to menu scene
    void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Login");
    }
    // Restart the game
    void restartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}
