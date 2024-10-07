using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonController : MonoBehaviour
{    
    public float shitCooldown = 1.0f;
    public float shitForce = 100.0f;

    public GameObject shitObject;
    public Transform shitOrigin;
    public GameObject warningObject;

    public float forwardCollisionWarningDistance = 10.0f;

    public Animator animator;

    // https://www.youtube.com/watch?v=fThb5M2OBJ8
    public float maxThrust = 500f;
    public float responsivness = 10f;

    [Header("Audio")]
    public AudioClip shitAudio;
    public AudioClip warningAudio;
    public AudioClip hitSound;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsivness;
        }
    }

    private float lastShit = 0.0f;
    private float yaw;
    private bool isAlive;

    private Rigidbody rb;
    private GameManager gameManager;
    private GameObject mesh;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        warningObject.SetActive(false);
        mesh = animator.gameObject.transform.parent.gameObject;
        isAlive = true;
        if (shitObject == null)
        {
            Debug.LogError("No shit object was assigned!");
        }

        SelectRandomSpawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(collision.impulse.normalized * 20.0f);
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
        // TODO: Flapping speed can be based on actual speed
        animator.SetFloat("flappingSpeed", 1.5f);
        animator.SetBool("steering", Mathf.Abs(rb.angularVelocity.y) > 0.5f);
        mesh.transform.localRotation = Quaternion.Euler(
            mesh.transform.localRotation.x,
            mesh.transform.localRotation.y,
            rb.angularVelocity.y * -20.0f
        );
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        rb.AddForce(transform.forward * maxThrust);
        rb.AddTorque(transform.up * yaw * responseModifier);

        CheckForwardCollision();
    }
    private void CheckForwardCollision()
    {
        bool left = Physics.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.forward - 0.5f * transform.right, forwardCollisionWarningDistance);
        bool middle = Physics.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.forward, forwardCollisionWarningDistance);
        bool right = Physics.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), transform.forward + 0.5f * transform.right, forwardCollisionWarningDistance);
        ToggleWarning(left || middle || right);
    }
    private void ToggleWarning(bool toggle)
    {
        warningObject.SetActive(toggle);
        if (toggle)
        {
            AudioManager.PlayAudioClip(warningAudio, transform, 0.3f, warningAudio.length);
        }
    }
    private void Shit()
    {
        var instance = Instantiate(shitObject);
        instance.transform.position = shitOrigin.position;
        instance.GetComponent<Rigidbody>().AddForce(-transform.up * shitForce);
        lastShit = Time.time;
        AudioManager.PlayAudioClip(shitAudio, transform, 0.5f);
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
        rb.drag = 0.0f;
        rb.angularDrag = 0.0f;
        animator.SetFloat("flappingSpeed", 0.0f);
        AudioManager.PlayAudioClip(hitSound, transform, 0.75f);
        gameManager.FinishGame();
    }

    private void SelectRandomSpawn() 
    {
        var spawn_points = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawn_points.Length > 0)
        {
            var rand_index = Random.Range(0, spawn_points.Length);
            Vector3 spawn_position;
            Quaternion spawn_rotation;

            spawn_points[rand_index].transform.GetPositionAndRotation(out spawn_position, out spawn_rotation);

            gameObject.transform.SetPositionAndRotation(spawn_position, spawn_rotation);
        }
    
    }
}
