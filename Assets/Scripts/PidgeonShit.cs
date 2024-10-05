using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonShit : MonoBehaviour
{
    public GameObject decalObject;
    public float minRandomScale = 1.0f;
    public float maxRandomScale = 2.0f;

    public float minRandomRotation = -5.0f;
    public float maxRandomRotation = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        var instance = Instantiate(decalObject);
        instance.transform.position = transform.position;

        float scale = Random.Range(minRandomScale, maxRandomScale);
        instance.transform.localScale = new Vector3(scale, scale, 1.0f);

        float rotation = Random.Range(minRandomRotation, maxRandomRotation);
        instance.transform.localEulerAngles = new Vector3(90.0f, rotation, rotation);

        Destroy(gameObject);
    }
}
