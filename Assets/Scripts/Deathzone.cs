// Deathzone Script with enum options (reset level, open GameObject, or kill player) and support for both trigger and collision
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{
	public enum DeathAction
	{
		ResetLevel,
		OpenGameObject,
		KillPlayer
	}

	public DeathAction action; // Enum to choose action
	public GameObject gameOverObject; // Reference to GameOver object, only used if action is OpenGameObject

	private MeshRenderer meshRenderer;
	private Collider zoneCollider;

	private void Start()
	{
		// Hide mesh renderer at start
		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer != null)
		{
			meshRenderer.enabled = false;
		}

		/*// Ensure collider is always set to trigger (optional if you want it to be a trigger)
		zoneCollider = GetComponent<Collider>();
		if (zoneCollider != null)
		{
			zoneCollider.isTrigger = true;
		}
		*/
	}

	private void HandlePlayerDeath(GameObject player)
	{
		switch (action)
		{
			case DeathAction.ResetLevel:
				// Reset the level
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				break;

			case DeathAction.OpenGameObject:
				// Activate GameOver object
				if (gameOverObject != null)
				{
					gameOverObject.SetActive(true);
				}
				else
				{
					Debug.LogWarning("No GameOver object assigned!");
				}
				break;

			case DeathAction.KillPlayer:
				// Call KillPlayer on the PlayerDeath script
				PlayerDeath playerDeath = player.GetComponent<PlayerDeath>();
				if (playerDeath != null)
				{
					playerDeath.KillPlayer();
				}
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			HandlePlayerDeath(other.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			HandlePlayerDeath(collision.gameObject);
		}
	}
}
