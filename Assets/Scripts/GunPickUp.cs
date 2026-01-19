using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    [SerializeField] private string gunChildName = "Gun";
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float destroyDelay = 0f;
    [SerializeField] private bool disableOnPickup = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Transform gunTransform = FindChildByName(other.transform, gunChildName);
        if (gunTransform == null)
        {
            Debug.LogWarning($"GunPickUp: Could not find child named '{gunChildName}' under {other.name}.");
            return;
        }

        gunTransform.gameObject.SetActive(true);

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

    static Transform FindChildByName(Transform root, string childName)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == childName)
            {
                return children[i];
            }
        }

        return null;
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
