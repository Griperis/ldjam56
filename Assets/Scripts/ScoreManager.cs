using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public SimpleRuntimeUI inGameUi;
    int totalScore = 0;

    void Awake()
    {
        if (inGameUi == null) 
        {
            Debug.LogError("Reference to UI not set in ScoreManager!");
        }
    }

    public void AddScore(int inScore) 
    {
        totalScore += inScore;

        if (inGameUi != null)
        {
            inGameUi.SetScore(totalScore);
        }

        Debug.LogFormat("Score is {0}", totalScore);
    }

    public void ResetScore()
    {
        totalScore = 0;
        inGameUi.SetScore(totalScore);
    }

    int GetScore() 
    {
        return totalScore;
    }
}
