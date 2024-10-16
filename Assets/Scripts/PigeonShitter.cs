using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PigeonShitter : MonoBehaviour
{
    public float shitCooldown = 1.0f;
    public float shitForce = 100.0f;

    [Header("Charged Shit")]
    public float maxShitCharge = 2.0f;
    public float chargedShitMultiplier = 1.5f;
    private float currentShitChargeTime = 0.0f;

    public GameObject shitObject;
    public Transform shitOrigin;

    [Header("Audio")]
    public AudioClip shitAudio;

    private float lastShit = 0.0f;
    private SimpleRuntimeUI inGameUi;


    private void Awake()
    {
        inGameUi = FindObjectOfType<SimpleRuntimeUI>();
        if (shitObject == null)
        {
            Debug.LogError("No shit object was assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetShitChargeInput())
        {
            currentShitChargeTime += Time.deltaTime;
            inGameUi.SetChargeProgress(GetNormalizedShitCharge());
        }
        if (IsShitInputReleased() && CanShit())
        {
            Shit();
        }
    }

    private void Shit()
    {
        var instance = Instantiate(shitObject);
        instance.transform.position = shitOrigin.position;
        var shitRb = instance.GetComponent<Rigidbody>();
        shitRb.AddForce(-transform.up * shitForce);
        shitRb.AddForce(transform.forward * 15.0f);

        var pidgeonShit = instance.GetComponent<PidgeonShit>();
        var normalizedShitCharge = GetNormalizedShitCharge();
        pidgeonShit.Modify(currentShitChargeTime * chargedShitMultiplier, normalizedShitCharge);

        currentShitChargeTime = 0.0f;
        lastShit = Time.time;
        inGameUi.DisableChargeProgress();

        AudioManager.PlayAudioClip(shitAudio, transform, 0.5f + normalizedShitCharge * 0.25f);
    }

    private bool CanShit()
    {
        //Debug.Log($"lastShit: {lastShit}; nextShit: {lastShit + shitCooldown}; time: {Time.time}");
        return lastShit + shitCooldown < Time.time || currentShitChargeTime > shitCooldown;
    }

    private bool IsShitInputReleased()
    {
        bool keyUp = Input.GetKeyUp(KeyCode.Space);
        bool anyTouch = false;

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended && touch.position.x < Screen.width * 2.0f / 3.0f && touch.position.x > Screen.width / 3.0f)
            {
                anyTouch = true;
                break;
            }
        }

        return anyTouch || keyUp;
    }

    private bool GetShitChargeInput()
    {
        // We prefer desktop input, but try to get touches as fallback
        var keyDown = Input.GetKey(KeyCode.Space);

        if (Input.touchCount == 0) return keyDown;

        bool anyTouch = false;
        foreach (var touch in Input.touches)
        {
            if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) && touch.position.x < Screen.width * 2.0f / 3.0f && touch.position.x > Screen.width / 3.0f)
            {
                anyTouch = true;
                break;
            }
        }
        return anyTouch || keyDown;
    }

    private float GetNormalizedShitCharge()
    {
        return currentShitChargeTime / maxShitCharge;
    }
}
