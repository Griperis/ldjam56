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

    public float normalizedModifier;

    public AudioClip[] hitSounds;

    private SphereCollider sphereCollider;
    private float assignedRandomScale;
    private float absoluteSizeModifier;

    private void Awake()
    {
        assignedRandomScale = Random.Range(minRandomScale, maxRandomScale);
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void Modify(float absoluteSizeModifier, float normalizedCharge)
    {
        normalizedModifier = normalizedCharge;
        this.absoluteSizeModifier = absoluteSizeModifier;
        
        var scaleModifier = 0.5f * new Vector3(normalizedCharge, normalizedCharge, normalizedCharge);
        transform.localScale += scaleModifier;
        sphereCollider.radius += GetScaleModifier(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        var instance = Instantiate(decalObject);
        instance.transform.position = transform.position;

        // This adjusts the z projection
        instance.size = new Vector3(instance.size.x, instance.size.y, instance.size.z + 5.0f * normalizedModifier);
        
        float scale = GetScaleModifier();
        instance.transform.localScale += new Vector3(scale, scale, 1.0f);

        float rotation = Random.Range(minRandomRotation, maxRandomRotation);
        instance.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, rotation);

        Destroy(gameObject);

        AudioManager.PlayRandomAudioClip(hitSounds, transform, 0.2f);
    }

    private float GetScaleModifier()
    {
        return assignedRandomScale + 2.0f * normalizedModifier;

    }
}
