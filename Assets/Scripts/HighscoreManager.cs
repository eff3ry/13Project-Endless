using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text names;
    [SerializeField] TMP_Text scores;
    private UserManager userManager;
    // This is set to constant to make the program more robust as this value should not change
    const int numOfScoresToShow = 5; 

    void Awake()
    {
        // Find the usermanager GameObject in the scene,
        // as it holds the current user
        userManager = FindObjectOfType<UserManager>();
    }

    // Whenever this GameObject is shown/enabled it will update the values
    void OnEnable()
    {
        updateScoreboard();
    }

    // Update the displayed scoreboard values
    void updateScoreboard()
    {
        List<userData> usersData = userManager.lastLoadedData;
        List<userData> sortedData = usersData.OrderByDescending(var => var.highScore).ToList();
        names.text = "";
        scores.text = "";

        // Only show 5 scores or if the list length is less than 5 use the list length
        int scoreNum = (sortedData.Count < numOfScoresToShow)? sortedData.Count : scoreNum = numOfScoresToShow;

        // Write the data to the text elements
        for (int i = 0; i < scoreNum; i++ )
        {
            names.text = names.text + $"{sortedData[i]._userName}\n";
            scores.text = scores.text + $"{sortedData[i].highScore}\n";
        }
        
    }
}
