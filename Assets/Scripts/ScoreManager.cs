using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public SimpleRuntimeUI inGameUi;
    int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (inGameUi == null) 
        {
            Debug.LogError("Reference to UI not set in ScoreManager!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    int GetScore() 
    {
        return totalScore;
    }
}
