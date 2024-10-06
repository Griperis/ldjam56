using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip buttonHoverClip;
    public AudioClip buttonClickClip;

    public void AddButtonSounds(UIDocument uiDocument)
    {
        List<Button> buttons = uiDocument.rootVisualElement.Query<Button>().ToList();
        foreach (var btn in buttons)
        {
            btn.RegisterCallback<MouseEnterEvent>(ButtonHovered);
            btn.RegisterCallback<ClickEvent>(ButtonClicked);
        }
    }

    private void ButtonHovered(MouseEnterEvent evt)
    {
        AudioManager.PlayAudioClip(buttonHoverClip, transform, 0.5f, destroyOnLoad: false);
    }

    private void ButtonClicked(ClickEvent evt)
    {
        AudioManager.PlayAudioClip(buttonClickClip, transform, 1.0f, destroyOnLoad: false);

    }
}
