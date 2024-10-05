using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonController : MonoBehaviour
{    
    public float speed = 20.0f;
    public float rotationSpeed = 5.0f;
    public float shitCooldown = 1.0f;

    Vector3 direction = Vector3.forward;
    float lastShit = 0.0f;

    public GameObject shitObject;
    public Transform shitOrigin;

    void Start()
    {
        if (shitObject == null)
        {
            Debug.LogError("No shit object was assigned!");
        }
    }

    void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(transform.rotation.x, (transform.rotation.y - rotationSpeed) * Time.deltaTime, transform.rotation.z));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(transform.rotation.x, (transform.rotation.y + rotationSpeed) * Time.deltaTime, transform.rotation.z));

        }
        if (Input.GetKeyDown(KeyCode.Space) && CanShit())
        {
            Shit();
        }
    }

    private void Shit()
    {
        var instance = Instantiate(shitObject);
        instance.transform.position = shitOrigin.position;
        lastShit = Time.time;
    }

    private bool CanShit()
    {
        //Debug.Log($"lastShit: {lastShit}; nextShit: {lastShit + shitCooldown}; time: {Time.time}");
        return lastShit + shitCooldown < Time.time;
    }
}
