using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonAnimator : MonoBehaviour
{
    public AudioClip flapAudioClip;

    public void WingFlap()
    {
        AudioManager.PlayAudioClip(flapAudioClip, transform, 0.5f);
    }
}
