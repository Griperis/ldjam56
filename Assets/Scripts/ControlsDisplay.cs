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

    private void Start()
    {
        fadeTimer = fadeStartTime;
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
        for (int i = 0; i < controls.Length; ++i)
        {
            //controls[i].a -= fadeSpeed * Time.deltaTime;
            var currentColor = controls[i].color;
            controls[i].color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - Time.deltaTime * fadeSpeed); 
            if (controls[i].color.a <= 0)
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

        if (Input.GetKey(KeyCode.Space))
        {
            TintAlpha(2);
        }
    }

    private void TintAlpha(int indicator)
    {
        var current = controls[indicator].color;
        controls[indicator].color = new Color(current.r, current.g, current.b, 0.5f);
    }

}
