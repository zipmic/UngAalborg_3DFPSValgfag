// Trap Script (kills player on trigger or collision)
using UnityEngine;

public class Trap : MonoBehaviour
{
	private void KillPlayer(GameObject player)
	{
		if (player.CompareTag("Player"))
		{
			Destroy(player);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		KillPlayer(other.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		KillPlayer(collision.gameObject);
	}
}
