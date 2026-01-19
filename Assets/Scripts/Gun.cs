using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
	[Header("Gun Settings")]
	public Rigidbody bulletPrefab; // Kuglens prefab (med Rigidbody)
	public float bulletSpeed = 20f; // Kuglens hastighed
	public float fireRate = 0.5f; // Skudhastighed i sekunder
	public Transform bulletSpawnTransform;
	public float spawnDistance = 1f; // Afstand fra kameraet, hvor kuglen spawner
	public AudioClip shootSound; // Lydeffekt for skuddet
	public float bulletLifetime = 5f; // Tiden før kuglen destrueres

	[Header("Ammo")]
	public int magazineSize = 12; // Antal skud i et magasin
	public int maxAmmo = 120; // Max samlet ammo (magasin + reserve)
	public int startingReserveAmmo = 60; // Start ammo i reserve
	public float reloadTime = 1.5f; // Tid i sekunder før reload er færdig
	public AudioClip reloadSound; // Lydeffekt for reload
	public TextMeshProUGUI ammoText; // UI tekst til ammo (valgfri)
	public string AmmoTextPrefix = "Ammo: ";

	[Header("Effects")]
	public Vector2 pitchRange = new Vector2(-2f, 2f); // Range for random pitch

	private float nextFireTime = 0f; // Tidspunkt for næste skud
	private AudioSource audioSource; // AudioSource til lydeffekten
	private int currentMagAmmo;
	private int reserveAmmo;
	private bool isReloading;

	void Start()
	{
		// Find AudioSource på våbnet, hvis den findes
		audioSource = GetComponent<AudioSource>();
		currentMagAmmo = Mathf.Clamp(magazineSize, 0, magazineSize);
		reserveAmmo = Mathf.Clamp(startingReserveAmmo, 0, Mathf.Max(0, maxAmmo - currentMagAmmo));
		UpdateAmmoUI();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			TryReload();
		}

		if (isReloading)
		{
			return;
		}

		// Tjek for venstre klik og om det er tid til at skyde
		if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
		{
			if (currentMagAmmo > 0)
			{
				Shoot();
				nextFireTime = Time.time + fireRate; // Opdaterer næste skudstidspunkt
			}
		}
	}

	void Shoot()
	{
		// Bestem spawn-positionen baseret på kameraets position og retning
		Vector3 spawnPosition = bulletSpawnTransform.position + bulletSpawnTransform.forward * spawnDistance;

		// Instantiér kuglen og sæt dens retning og hastighed
		Rigidbody bulletInstance = Instantiate(bulletPrefab, spawnPosition, bulletSpawnTransform.rotation);
		bulletInstance.linearVelocity = bulletSpawnTransform.forward * bulletSpeed;

		currentMagAmmo = Mathf.Max(0, currentMagAmmo - 1);
		UpdateAmmoUI();

		// Destruer kuglen efter bulletLifetime sekunder
		Destroy(bulletInstance.gameObject, bulletLifetime);

		// Afspil lydeffekt med random pitch, hvis en AudioSource findes og et skud-lydklip er sat
		if (audioSource != null && shootSound != null)
		{
			audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y); // Random pitch
			audioSource.PlayOneShot(shootSound);
		}

		Debug.Log("Skud affyret fremad.");
	}

	void TryReload()
	{
		if (isReloading)
		{
			return;
		}

		if (currentMagAmmo >= magazineSize)
		{
			return;
		}

		if (reserveAmmo <= 0)
		{
			return;
		}

		StartCoroutine(Reload());
	}

	System.Collections.IEnumerator Reload()
	{
		isReloading = true;

		if (audioSource != null && reloadSound != null)
		{
			audioSource.pitch = 1f;
			audioSource.PlayOneShot(reloadSound);
		}

		yield return new WaitForSeconds(reloadTime);

		int neededAmmo = magazineSize - currentMagAmmo;
		int ammoToLoad = Mathf.Min(neededAmmo, reserveAmmo);
		currentMagAmmo += ammoToLoad;
		reserveAmmo -= ammoToLoad;

		UpdateAmmoUI();
		isReloading = false;
	}

	public void AddAmmo(int amount)
	{
		if (amount <= 0)
		{
			return;
		}

		int totalAmmo = currentMagAmmo + reserveAmmo;
		int spaceLeft = Mathf.Max(0, maxAmmo - totalAmmo);
		int ammoToAdd = Mathf.Min(amount, spaceLeft);
		reserveAmmo += ammoToAdd;
		UpdateAmmoUI();
	}

	void UpdateAmmoUI()
	{
		if (ammoText == null)
		{
			return;
		}

		ammoText.text = $"{AmmoTextPrefix}{currentMagAmmo}/{currentMagAmmo + reserveAmmo}";
	}

	void OnDrawGizmos()
	{
		// Tjek om kameraTransform er sat
		if (bulletSpawnTransform != null)
		{
			// Tegn en rød linje fra kameraets position til spawn-positionen
			Gizmos.color = Color.red;
			Vector3 spawnPosition = bulletSpawnTransform.position + bulletSpawnTransform.forward * spawnDistance;
			Gizmos.DrawLine(bulletSpawnTransform.position, spawnPosition);
		}
	}
}
