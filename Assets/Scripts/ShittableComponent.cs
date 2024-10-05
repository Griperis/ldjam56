using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittableComponent : MonoBehaviour
{
    public int score = 0;

    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag("GameController");

        if (objects.Length > 0)
        {
            scoreManager = objects[0].GetComponent<ScoreManager>();
        }

        if (scoreManager == null)
        {
            Debug.LogError("Score manager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleHit() 
    {
        if (scoreManager == null) 
        {
            return;
        }

        scoreManager.AddScore(score);

        //TODO: Handle animations, sounds and shit here
    }
}
