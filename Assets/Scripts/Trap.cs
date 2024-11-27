// Trap Script (kills player on trigger or collision)
using UnityEngine;

public class Trap : MonoBehaviour
{
	private void KillPlayer(GameObject player)
	{
		if (player.CompareTag("Player"))
		{
			player.GetComponent<PlayerDeath>().KillPlayer();
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
