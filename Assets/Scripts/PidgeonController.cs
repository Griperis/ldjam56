using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonController : MonoBehaviour
{    
    public float speed = 20.0f;
    public float rotationSpeed = 5.0f;

    public float shitCooldown = 1.0f;
    public float shitForce = 100.0f;

    Vector3 direction = Vector3.forward;
    float lastShit = 0.0f;

    public GameObject shitObject;
    public Transform shitOrigin;


    // https://www.youtube.com/watch?v=fThb5M2OBJ8
    public float maxThrust = 500f;
    public float responsivness = 10f;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsivness;
        }
    }
    private float yaw;
    private bool isAlive;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        isAlive = true;
        if (shitObject == null)
        {
            Debug.LogError("No shit object was assigned!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Die();
    }

    void Update()
    {
        if (!isAlive) return;


        yaw = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && CanShit())
        {
            Shit();
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        rb.AddForce(transform.forward * maxThrust);
        rb.AddTorque(transform.up * yaw * responseModifier);
    }


    private void Shit()
    {
        var instance = Instantiate(shitObject);
        instance.transform.position = shitOrigin.position;
        instance.GetComponent<Rigidbody>().AddForce(-transform.up * shitForce);
        lastShit = Time.time;
    }

    private bool CanShit()
    {
        //Debug.Log($"lastShit: {lastShit}; nextShit: {lastShit + shitCooldown}; time: {Time.time}");
        return lastShit + shitCooldown < Time.time;
    }

    private void Die()
    {
        // already dead
        if (!isAlive) return;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        isAlive = false;
    }
}
