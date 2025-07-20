using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    public AudioClip clip;
    public float tiltThreshold = 0.5f; // < 0.5 means significantly tilted
    public float angularVelocityThreshold = 1.0f;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool willHaveImpact = false;
    private bool soundPlayed = false;
    private bool isKnockedOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isKnockedOver) return;

        float tilt = Vector3.Dot(transform.up, Vector3.up);

        if (!willHaveImpact && tilt < tiltThreshold)
        {
            willHaveImpact = true;
        }
    }

    void Update()
    {
        if (isKnockedOver || !willHaveImpact) return;

        if (!soundPlayed && rb.angularVelocity.magnitude > angularVelocityThreshold)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
            soundPlayed = true;
            isKnockedOver = true; // Stop further checks
        }
    }
}
