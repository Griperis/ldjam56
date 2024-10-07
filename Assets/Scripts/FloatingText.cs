using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float fadeTime = 1.5f;
    public float fadeSpeed = 3.0f;
    public float moveSpeed = 2.0f;
    public float randomXSpeed = 0.5f;

    public TextMeshPro textMeshPro;

    private float fadeTimer = 0.0f;
    private Color baseTextColor;
    private Color baseOutlineColor;
    private float randomXMoveSpeed = 0.0f;
    private float baseFontSize = 0;

    public void Setup(string text, Color? color = null, Color? outlineColor = null)
    {
        Setup_Internal(text, color, outlineColor);
    }
    private void Setup_Internal(string text, Color? color = null, Color? outlineColor = null)
    {
        fadeTimer = fadeTime;
        randomXMoveSpeed = Random.Range(-randomXSpeed, randomXSpeed);
        transform.position += new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
        baseFontSize = textMeshPro.fontSize;

        if (color != null)
        {
            baseTextColor = color.Value;
        }
        else
        {
            baseTextColor = textMeshPro.color;
        }

        if (outlineColor != null)
        {
            baseOutlineColor = outlineColor.Value;
            textMeshPro.outlineWidth = 0.5f;
        }
        else
        {
            baseOutlineColor = textMeshPro.outlineColor;
        }

        textMeshPro.color = baseTextColor;
        textMeshPro.outlineColor = baseOutlineColor;
        textMeshPro.SetText(text);
    }

    private void Update()
    {
        transform.position += new Vector3(randomXMoveSpeed, 0.0f, moveSpeed) * Time.deltaTime;
        fadeTimer -= Time.deltaTime;
        if (fadeTimer < 0)
        {
            baseTextColor.a -= fadeSpeed * Time.deltaTime;
            textMeshPro.color = baseTextColor;
        }
        if (baseTextColor.a <= 0)
        {
            Destroy(gameObject);
        } 

    }
}
