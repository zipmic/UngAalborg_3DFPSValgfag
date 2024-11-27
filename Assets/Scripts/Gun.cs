using UnityEngine;

public class Gun : MonoBehaviour
{
	[Header("Gun Settings")]
	public Rigidbody bulletPrefab; // Kuglens prefab (med Rigidbody)
	public float bulletSpeed = 20f; // Kuglens hastighed
	public float fireRate = 0.5f; // Skudhastighed i sekunder
	public Transform cameraTransform; // Referencen til kameraet
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
		// Bestem spawn-positionen baseret på kameraets position og retning
		Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

		// Instantiér kuglen og sæt dens retning og hastighed
		Rigidbody bulletInstance = Instantiate(bulletPrefab, spawnPosition, cameraTransform.rotation);
		bulletInstance.velocity = cameraTransform.forward * bulletSpeed;

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

		Debug.Log("Skud affyret fremad.");
	}

	void OnDrawGizmos()
	{
		// Tjek om kameraTransform er sat
		if (cameraTransform != null)
		{
			// Tegn en rød linje fra kameraets position til spawn-positionen
			Gizmos.color = Color.red;
			Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;
			Gizmos.DrawLine(cameraTransform.position, spawnPosition);
		}
	}
}
