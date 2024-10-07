using Unity.VisualScripting;
using UnityEngine;

public class ControlsDisplay : MonoBehaviour
{
    public float fadeStartTime = 3.0f;
    public float fadeSpeed = 3.0f;
    // This has to have the right order
    [Header("Controls (L, R, Space)")]
    public SpriteRenderer[] controls;

    private float fadeTimer;
    private Color[] spriteColors;

    private void Start()
    {
        fadeTimer = fadeStartTime;
        spriteColors = new Color[controls.Length];
        for (int i = 0; i < controls.Length; ++i)
        {
            spriteColors[i] = controls[i].color;
        }
    }

    private void Update()
    {
        fadeTimer -= Time.deltaTime;
        if (fadeTimer < 0)
        {
            UpdateSpritesAlpha();
        }
        else
        {
            ReactToInput();
        }
    }

    private void UpdateSpritesAlpha()
    {
        for (int i = 0; i < spriteColors.Length; ++i)
        {
            spriteColors[i].a -= fadeSpeed * Time.deltaTime;
            controls[i].color = spriteColors[i];
            if (spriteColors[i].a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReactToInput()
    {
        float yaw = Input.GetAxis("Horizontal");
        if (yaw > 0.005f)
        {
            TintAlpha(1);
        }
        else if (yaw < -0.005f)
        {
            TintAlpha(0);
        }
        else
        {
            ResetAlpha(0);
            ResetAlpha(1);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            TintAlpha(2);
        }
        else
        {
            ResetAlpha(2);
        }
    }

    private void TintAlpha(int indicator)
    {
        var current = controls[indicator].color;
        controls[indicator].color = new Color(current.r, current.g, current.b, 0.5f);
    }

    private void ResetAlpha(int indicator)
    {
        controls[indicator].color = controls[indicator].color;
    }
}
