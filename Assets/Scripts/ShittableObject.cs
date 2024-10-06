using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittableObject : MonoBehaviour
{
    public int score = 10;

    ScoreManager scoreManager;
    Outline outline;
    Tasker tasker;

    public void ToggleOutline(bool value)
    {
        outline.enabled = value;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public void SetOutlineColor(Color color)
    {
        outline.OutlineColor = color;
    }

    private void Awake()
    {
        outline = GetComponent<Outline>();
        tasker = FindObjectOfType<Tasker>();
        scoreManager = FindObjectOfType<ScoreManager>();
        if (outline == null)
        {
            throw new MissingComponentException($"Outline missing on {gameObject.name}!");
        }

        if (tasker == null)
        {
            throw new MissingComponentException("Tasker missing in scene!");
        }

        if (scoreManager == null)
        {
            throw new MissingComponentException("ScoreManager missing in scene!");

        }

        ToggleOutline(false);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PigeonShit"))
        {
            HandleHit();
        }
    }

    private void HandleHit() 
    {
        tasker.ShittableObjectHit(this);
        scoreManager.AddScore(score);

        //TODO: Handle animations, sounds and shit here
    }

}
