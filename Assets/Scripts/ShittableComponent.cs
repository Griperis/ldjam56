using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittableComponent : MonoBehaviour
{
    public int score = 10;
    public int shitHitLimit = 1;

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
    private void OnTriggerEnter(Collider other)
    {
        if (shitHitLimit == 0)
        {
            return;
        }

        HandleHit();
        shitHitLimit--;
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
