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
    [SerializeField] Player player;


    private UserManager userManager;
    private UiController uiController;
    void Awake()
    {
        uiController = FindObjectOfType<UiController>();
        userManager = FindObjectOfType<UserManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(delegate { Debug.Log("Restart"); restartGame(); });
        saveScoreButton.onClick.AddListener(delegate {saveHighScore(player.score); });
        backToMenuButton.onClick.AddListener(delegate {backToMenu();});
    }

    void OnEnable()
    {
        //show save score button only if score > highscore
        saveScoreButton.gameObject.SetActive(player.score > userManager.currentUser.highScore);
        saveScoreButton.interactable = true;

        //Update Scores when loaded
        highscoreText.text = $"Highscore: {userManager.currentUser.highScore}";
        scoreText.text = $"Score: {player.score}";
    }

    void saveHighScore(int score)
    {
        userManager.lastLoadedData[userManager.currentUserIndex].highScore = score;
        userManager.currentUser = userManager.lastLoadedData[userManager.currentUserIndex];
        userManager.saveData(userManager.lastLoadedData);
        saveScoreButton.interactable = false;
        highscoreText.text = $"Highscore: {userManager.currentUser.highScore}";
    }

    void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Login");
    }
    void restartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}
