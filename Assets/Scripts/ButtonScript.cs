using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonScript : MonoBehaviour
{
    [Header("Button Settings")]
    public GameObject objectToActivate; // Object to activate when the button is pressed

    private bool activated = false;

    public GameObject ButtonOn, ButtonOff;

    [Header("Audio Settings")]
    public AudioClip buttonSound; // Sound to play when button is activated
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Check if a sound clip is assigned
        if (buttonSound == null)
        {
            Debug.LogWarning("No button sound assigned to " + gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        if (objectToActivate != null)
        {
            // If objectToActivate is inactive and has a MeshRenderer or children, draw Gizmos
            if (!objectToActivate.activeInHierarchy)
            {
                // Draw Gizmos for parent object
                DrawGizmoForGameObject(objectToActivate);
            }

            // Draw a line from the button to the objectToActivate
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, objectToActivate.transform.position);
        }
    }

    private void DrawGizmoForGameObject(GameObject target)
    {
        MeshFilter meshFilter = target.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f); // Transparent green
            Gizmos.DrawWireMesh(meshFilter.sharedMesh, target.transform.position, target.transform.rotation, target.transform.localScale);
        }

        // If the target has children, draw Gizmos for each child recursively
        foreach (Transform child in target.transform)
        {
            DrawGizmoForGameObject(child.gameObject);
        }
    }

    private void ActivateButton()
    {
        if (!activated)
        {
            activated = true;

            // Update button visuals
            if (ButtonOff != null) ButtonOff.SetActive(false);
            if (ButtonOn != null) ButtonOn.SetActive(true);

            // Play button activation sound
            if (audioSource != null && buttonSound != null)
            {
                audioSource.PlayOneShot(buttonSound);
            }

            // Activate the target object
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateButton();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ActivateButton();
        }
    }
}
