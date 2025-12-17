using UnityEngine;

public class PickUpCollectable : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private AudioClip pickupSound;

    private ScoreManager scoreManager;

    private void Awake()
    {
        // Cache the ScoreManager so we always have a target for coin updates.
        scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in the scene. Ensure one exists before collecting coins.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (scoreManager == null)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (pickupSound != null)
        {
            scoreManager.PlayPickUpSound(pickupSound);
        }

        scoreManager.AddCoins(coinValue);
        Destroy(gameObject);
    }
}
