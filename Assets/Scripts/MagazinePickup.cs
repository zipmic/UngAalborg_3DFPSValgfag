using UnityEngine;

public class MagazinePickup : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int ammoAmount = 12;

    [Header("Pickup")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float destroyDelay = 0f;
    [SerializeField] private bool disableOnPickup = true;

    private bool hasPickedUp;

    void OnTriggerEnter(Collider other)
    {
        if (hasPickedUp)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        Gun gun = other.GetComponentInChildren<Gun>(true);
        if (gun == null)
        {
            Debug.LogWarning($"MagazinePickup: No Gun found under {other.name}.");
            return;
        }

        gun.AddAmmo(ammoAmount);
        hasPickedUp = true;

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            if (destroyDelay <= 0f)
            {
                destroyDelay = pickupSound.length;
            }
        }

        if (disableOnPickup)
        {
            SetPickupEnabled(false);
        }

        Destroy(gameObject, Mathf.Max(0f, destroyDelay));
    }

    void SetPickupEnabled(bool enabled)
    {
        Collider pickupCollider = GetComponent<Collider>();
        if (pickupCollider != null)
        {
            pickupCollider.enabled = enabled;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = enabled;
        }
    }
}
