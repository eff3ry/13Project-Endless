using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text names;
    [SerializeField] TMP_Text scores;
    UserManager userManager;
    const int numOfScoresToShow = 5; // constant for a more robust program
    void Awake()
    {
        userManager = FindObjectOfType<UserManager>();
    }

    //when ever this object is shown/enabled it will update the values
    void OnEnable()
    {
        updateScoreboard();
    }

    //update the scoreboard values
    void updateScoreboard()
    {
        List<userData> usersData = userManager.lastLoadedData;
        List<userData> sortedData = usersData.OrderByDescending(var => var.highScore).ToList();
        names.text = "";
        scores.text = "";
        int scoreNum;

        //only show 5 scores or if the list is less that 5 use the list length
        if (sortedData.Count < numOfScoresToShow)
        {
            scoreNum = sortedData.Count;
        } else
        {
            scoreNum = numOfScoresToShow;
        }

        //write the data to the text elements
        for (int i = 0; i < scoreNum; i++ )
        {
            names.text = names.text + $"{sortedData[i]._userName}\n";
            scores.text = scores.text + $"{sortedData[i].highScore}\n";
        }
        
    }
}
