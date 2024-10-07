using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittableObject : MonoBehaviour
{
    public int score = 10;

    ScoreManager scoreManager;
    Outline outline;
    Tasker tasker;

    public void Highlight(Color color)
    {
        outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = color;
        outline.OutlineWidth = 3.0f;
    }

    public void DisableHighlight()
    {
        outline.OutlineColor = Color.white;
        outline.OutlineWidth = 1.0f;
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

        DisableHighlight();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PigeonShit"))
        {
            HandleHit(collision.collider.GetComponent<PidgeonShit>());
        }
    }

    private void HandleHit(PidgeonShit collider) 
    {
        var finalScore = Mathf.RoundToInt(collider.sizeModifier * score);
        tasker.ShittableObjectHit(this);
        scoreManager.AddScore(finalScore);
        FloatingTextManager.CreateFloatingText(collider.transform, $"+{finalScore}");

        //TODO: Handle animations, sounds and shit here
    }

}
