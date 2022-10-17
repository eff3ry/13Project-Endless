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
    int numOfScoresToShow = 5;
    void Awake()
    {
        userManager = FindObjectOfType<UserManager>();
    }

    void OnEnable()
    {
        updateScoreboard();
    }

    void updateScoreboard()
    {
        List<userData> usersData = userManager.lastLoadedData;
        List<userData> sortedData = usersData.OrderByDescending(var => var.highScore).ToList();
        names.text = "";
        scores.text = "";
        int scoreNum;
        if (sortedData.Count < numOfScoresToShow)
        {
            scoreNum = sortedData.Count;
        } else
        {
            scoreNum = numOfScoresToShow;
        }
        for (int i = 0; i < scoreNum; i++ )
        {
            names.text = names.text + $"{sortedData[i]._userName}\n";
            scores.text = scores.text + $"{sortedData[i].highScore}\n";
        }
        
    }
}
