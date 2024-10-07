using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PidgeonShit : MonoBehaviour
{
    public DecalProjector decalObject;
    public float minRandomScale = 1.0f;
    public float maxRandomScale = 2.0f;

    public float minRandomRotation = -5.0f;
    public float maxRandomRotation = 5.0f;

    public float sizeModifier = 1.0f;
    public float normalizedShitCharge;

    public AudioClip[] hitSounds;

    public void Modify(float sizeModifier, float normalizedCharge)
    {
        this.sizeModifier = sizeModifier;
        normalizedShitCharge = normalizedCharge;
        transform.localScale = transform.localScale + 0.5f * new Vector3(normalizedCharge, normalizedCharge, normalizedCharge); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        var instance = Instantiate(decalObject);
        instance.transform.position = transform.position;

        // This adjusts the z projection
        instance.size = new Vector3(instance.size.x, instance.size.y, instance.size.z + sizeModifier);

        float randomScale = Random.Range(minRandomScale, maxRandomScale);
        instance.transform.localScale += new Vector3(randomScale, randomScale, 1.0f);
        instance.transform.localScale += 2.0f * new Vector3(normalizedShitCharge, normalizedShitCharge, 0.0f);

        float rotation = Random.Range(minRandomRotation, maxRandomRotation);
        instance.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, rotation);

        Destroy(gameObject);

        AudioManager.PlayRandomAudioClip(hitSounds, transform, 0.2f);
    }
}
