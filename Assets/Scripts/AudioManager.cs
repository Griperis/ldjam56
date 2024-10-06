using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceObject;

    private static AudioSource _audioSource;
    private static Dictionary<string, (float lastPlayTime, float cooldown)> _audioSourceCooldowns = new Dictionary<string, (float, float)>();

    private void Awake()
    {
        _audioSource = audioSourceObject;
    }

    public static void PlayAudioClip(AudioClip audioClip, Transform transform, float volume, float cooldown = 0.0f, bool destroyOnLoad = true)
    {
        if (cooldown > 0.0f && CheckCooldown(audioClip, cooldown))
        {
            return;
        }

        var audioSource = Instantiate(_audioSource, transform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        if (!destroyOnLoad)
        {
            DontDestroyOnLoad(audioSource);
        }
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public static void PlayRandomAudioClip(AudioClip[] audioClips, Transform transform, float volume)
    {
        PlayAudioClip(audioClips[Random.Range(0, audioClips.Length)], transform, volume);
    }

    private static bool CheckCooldown(AudioClip audioClip, float cooldown)
    {
        if (_audioSourceCooldowns.ContainsKey(audioClip.name))
        {
            var entry = _audioSourceCooldowns[audioClip.name];
            if (Time.time - entry.lastPlayTime > entry.cooldown)
            {
                _audioSourceCooldowns.Remove(audioClip.name);
                return false;
            }
            return true;
        }
        else
        {
            _audioSourceCooldowns.Add(audioClip.name, (Time.time, cooldown));
            return false;
        }

    }
}
