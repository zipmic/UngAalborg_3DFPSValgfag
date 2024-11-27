using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum BulletDirection
    {
        Backward,
        Down,
        Forward,
        Left,
        Right,
        Up
    }

    [Header("Gun Settings")]
    public Rigidbody bulletPrefab; // Kuglens prefab (med Rigidbody)
    public float bulletSpeed = 20f; // Kuglens hastighed
    public float fireRate = 0.5f; // Skudhastighed i sekunder
    public Transform cameraTransform; // Referencen til kameraet
    public BulletDirection bulletDirection = BulletDirection.Forward; // Retning for kuglen
    public float spawnDistance = 1f; // Afstand fra kameraet, hvor kuglen spawner
    public AudioClip shootSound; // Lydeffekt for skuddet
    public float bulletLifetime = 5f; // Tiden før kuglen destrueres

    [Header("Effects")]
    public ParticleSystem muzzleFlash; // Muzzle flash partikel (valgfri)
    public Vector2 pitchRange = new Vector2(-2f, 2f); // Range for random pitch

    private float nextFireTime = 0f; // Tidspunkt for næste skud
    private AudioSource audioSource; // AudioSource til lydeffekten

    void Start()
    {
        // Find AudioSource på våbnet, hvis den findes
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Tjek for venstre klik og om det er tid til at skyde
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Opdaterer næste skudstidspunkt
        }
    }

    void Shoot()
    {
        // Bestem retning og rotation for kuglen baseret på enum
        Vector3 direction = GetDirection();
        Quaternion rotation = GetRotation();

        // Bestem spawn-positionen baseret på kameraets position og retning
        Vector3 spawnPosition = cameraTransform.position + direction * spawnDistance;

        // Instantiér kuglen og sæt dens rotation
        Rigidbody bulletInstance = Instantiate(bulletPrefab, spawnPosition, rotation);
        bulletInstance.velocity = direction * bulletSpeed;

        // Destruer kuglen efter bulletLifetime sekunder
        Destroy(bulletInstance.gameObject, bulletLifetime);

        // Afspil lydeffekt med random pitch, hvis en AudioSource findes og et skud-lydklip er sat
        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y); // Random pitch
            audioSource.PlayOneShot(shootSound);
        }

        // Afspil muzzle flash, hvis den er sat
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        Debug.Log("Skud affyret i retning: " + bulletDirection);
    }

    Vector3 GetDirection()
    {
        // Returnér retningen baseret på bulletDirection
        switch (bulletDirection)
        {
            case BulletDirection.Backward:
                return -cameraTransform.forward;
            case BulletDirection.Down:
                return -cameraTransform.up;
            case BulletDirection.Forward:
                return cameraTransform.forward;
            case BulletDirection.Left:
                return -cameraTransform.right;
            case BulletDirection.Right:
                return cameraTransform.right;
            case BulletDirection.Up:
                return cameraTransform.up;
            default:
                return cameraTransform.forward; // Standard retning
        }
    }

    Quaternion GetRotation()
    {
        // Returnér rotationen baseret på bulletDirection
        switch (bulletDirection)
        {
            case BulletDirection.Backward:
                return Quaternion.LookRotation(-cameraTransform.forward);
            case BulletDirection.Down:
                return Quaternion.LookRotation(-cameraTransform.up);
            case BulletDirection.Forward:
                return Quaternion.LookRotation(cameraTransform.forward);
            case BulletDirection.Left:
                return Quaternion.LookRotation(-cameraTransform.right);
            case BulletDirection.Right:
                return Quaternion.LookRotation(cameraTransform.right);
            case BulletDirection.Up:
                return Quaternion.LookRotation(cameraTransform.up);
            default:
                return Quaternion.LookRotation(cameraTransform.forward); // Standard rotation
        }
    }

    void OnDrawGizmos()
    {
        // Tjek om kameraTransform er sat
        if (cameraTransform != null)
        {
            // Bestem retningen for linjen
            Vector3 direction = GetDirection();

            // Tegn en rød linje fra kameraets position til spawn-positionen
            Gizmos.color = Color.red;
            Vector3 spawnPosition = cameraTransform.position + direction * spawnDistance;
            Gizmos.DrawLine(cameraTransform.position, spawnPosition);
        }
    }
}
