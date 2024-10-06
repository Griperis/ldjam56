using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentAudio : MonoBehaviour
{
    public AudioClip[] additionalEnvironmentAudio;
    public float additionalAudioTimerMin = 20.0f;
    public float additionalAudioTimerMax = 30.0f;

    public void Start()
    {
        StartCoroutine(PlayEnvironmentAudio(GetEnvironmentAudioDelay()));
    }

    private IEnumerator PlayEnvironmentAudio(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AudioManager.PlayRandomAudioClip(additionalEnvironmentAudio, transform, 0.3f);
        StartCoroutine(PlayEnvironmentAudio(GetEnvironmentAudioDelay()));
    }

    private float GetEnvironmentAudioDelay()
    {
        return Random.Range(additionalAudioTimerMin, additionalAudioTimerMax);
    }
}
