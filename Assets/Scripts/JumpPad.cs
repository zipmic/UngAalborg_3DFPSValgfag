using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Ensures AudioSource is present on the GameObject
public class JumpPad : MonoBehaviour
{
	[Header("Jump Pad Settings")]
	public float jumpPower = 10f; // Power of the jump force

	[Header("Audio Settings")]
	public AudioClip jumpSound; // Sound to play when the player jumps

	private AudioSource audioSource;

	void Awake()
	{
		// Get the AudioSource component
		audioSource = GetComponent<AudioSource>();

		// Warn if no AudioClip is assigned
		if (audioSource == null)
		{
			Debug.LogError("Missing AudioSource component on JumpPad.");
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Check if the object colliding with the jump pad is the player
		if (collision.gameObject.CompareTag("Player"))
		{
			// Find the player's Rigidbody
			Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

			if (playerRigidbody != null)
			{
				// Apply an upward force to the player
				Vector3 jumpForce = Vector3.up * jumpPower;
				playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z); // Reset vertical velocity
				playerRigidbody.AddForce(jumpForce, ForceMode.VelocityChange);

				// Play the jump sound
				if (jumpSound != null)
				{
					audioSource.PlayOneShot(jumpSound);
				}
			}
		}
	}
}
