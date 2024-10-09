// Button Script (works with both trigger and collision, shows wireframe of disabled object in editor)
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
	public GameObject replacementObject;
	public GameObject objectToActivate;

	private bool activated = false;

	private void OnDrawGizmos()
	{
		if (objectToActivate != null && !objectToActivate.activeInHierarchy)
		{
			MeshFilter meshFilter = objectToActivate.GetComponent<MeshFilter>();

			if (meshFilter != null)
			{
				Gizmos.color = new Color(0, 1, 0, 0.3f); // Transparent green
				Gizmos.DrawWireMesh(meshFilter.sharedMesh, objectToActivate.transform.position,
									objectToActivate.transform.rotation, objectToActivate.transform.localScale);
			}
		}

		if (objectToActivate != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, objectToActivate.transform.position);
		}
	}

	private void ActivateButton()
	{
		if (!activated)
		{
			activated = true;

			if (replacementObject != null)
			{
				Instantiate(replacementObject, transform.position, transform.rotation);
				Destroy(gameObject);
			}

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
