using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionWarning : MonoBehaviour
{
    public float warningObjectOffset = 2.0f;
    public GameObject warningObject;

    [Header("Audio")]
    public AudioClip warningAudio;

    private void Awake()
    {
        warningObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PigeonShit"))
        {
            return;
        }
        warningObject.SetActive(true);
        AudioManager.PlayAudioClip(warningAudio, transform, 0.3f, warningAudio.length);
    }

    private void OnTriggerExit(Collider other)
    {
        warningObject.SetActive(false);
    }


}
