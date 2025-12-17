using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TurretController : MonoBehaviour
{
    [Header("Turret Settings")]
    public Transform turretHead; // The part of the turret that rotates
    public float detectionRadius = 15f; // Radius to detect the player
    public float rotationSpeed = 2f; // Speed of rotation when idle
    public float minAngle = -30f; // Minimum angle for idle oscillation
    public float maxAngle = 30f; // Maximum angle for idle oscillation

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Bullet prefab
    public Transform[] shootPoints; // Array of shoot points
    public float fireRate = 0.5f; // Rate of fire
    public float bulletSpeed = 20f; // Speed of the bullets
    public AudioClip shootingSound; // Sound effect for shooting
    public float minPitch = 0.8f; // Minimum pitch for the shooting sound
    public float maxPitch = 1.2f; // Maximum pitch for the shooting sound

    [Header("Player Detection")]
    public LayerMask detectionLayers; // Layers to detect
    public Transform player; // Player reference

    private float currentAngle; // Current oscillation angle
    private bool increasingAngle = true; // Flag to determine oscillation direction
    private float nextFireTime = 0f; // Time to fire next bullet
    private bool playerDetected = false; // If the player is detected
    private int currentShootPointIndex = 0; // Tracks the current shoot point index

    private AudioSource audioSource; // AudioSource for playing sounds

    private void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Warn if no shooting sound is assigned
        if (shootingSound == null)
        {
            Debug.LogWarning("No shooting sound assigned on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (playerDetected)
        {
            TrackAndShootPlayer();
        }
        else
        {
            IdleMovement();
            DetectPlayer();
        }
    }

    private void IdleMovement()
    {
        if (increasingAngle)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            if (currentAngle >= maxAngle) increasingAngle = false;
        }
        else
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
            if (currentAngle <= minAngle) increasingAngle = true;
        }

        turretHead.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayers);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 directionToPlayer = (hit.transform.position - turretHead.position).normalized;
                float angleToPlayer = Vector3.SignedAngle(turretHead.forward, directionToPlayer, Vector3.up);

                if (angleToPlayer >= minAngle && angleToPlayer <= maxAngle)
                {
                    if (Physics.Raycast(turretHead.position, directionToPlayer, out RaycastHit rayHit, detectionRadius))
                    {
                        if (rayHit.collider.CompareTag("Player"))
                        {
                            player = hit.transform;
                            playerDetected = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void TrackAndShootPlayer()
    {
        if (player == null)
        {
            playerDetected = false;
            return;
        }

        Vector3 directionToPlayer = (player.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            playerDetected = false;
            player = null;
            return;
        }

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && shootPoints.Length > 0)
        {
            Transform shootPoint = shootPoints[currentShootPointIndex];
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootPoint.forward * bulletSpeed;
            }
            Destroy(bullet, 5f);

            // Play shooting sound with random pitch
            if (audioSource != null && shootingSound != null)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(shootingSound);
            }

            currentShootPointIndex = (currentShootPointIndex + 1) % shootPoints.Length;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (turretHead != null)
        {
            Vector3 origin = turretHead.position;

            // Min angle
            Gizmos.color = Color.green;
            Quaternion minRotation = Quaternion.Euler(0, minAngle, 0);
            Vector3 minDirection = minRotation * turretHead.forward * detectionRadius;
            Gizmos.DrawLine(origin, origin + minDirection);

            // Max angle
            Gizmos.color = Color.blue;
            Quaternion maxRotation = Quaternion.Euler(0, maxAngle, 0);
            Vector3 maxDirection = maxRotation * turretHead.forward * detectionRadius;
            Gizmos.DrawLine(origin, origin + maxDirection);
        }
    }
}
