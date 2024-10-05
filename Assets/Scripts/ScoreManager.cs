using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int inScore) 
    {
        totalScore += inScore;
        Debug.LogFormat("Score is {0}", totalScore);
    }

    int GetScore() 
    {
        return totalScore;
    }
}
