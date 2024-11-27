using UnityEngine;

public class MovingBlock : MonoBehaviour
{
	[Header("Movement Settings")]
	public float speed = 2f; // Hastighed p� bev�gelsen
	public float distance = 5f; // Afstanden, blokken bev�ger sig

	private Vector3 startPosition; // Startpositionen for blokken
	private Vector3 targetPosition; // Slutpositionen for blokken
	private bool movingForward = true; // Om blokken bev�ger sig fremad

	void Start()
	{
		// Gem startpositionen og beregn slutpositionen
		startPosition = transform.position;
		targetPosition = startPosition + transform.forward * distance;
	}

	void Update()
	{
		// Bev�g blokken frem og tilbage
		if (movingForward)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

			// Skift retning, hvis den n�r slutpositionen
			if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
			{
				movingForward = false;
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

			// Skift retning, hvis den n�r startpositionen
			if (Vector3.Distance(transform.position, startPosition) < 0.01f)
			{
				movingForward = true;
			}
		}
	}

	void OnDrawGizmos()
	{
		// Tegn en linje, der viser bev�gelsen mellem start og slut
		Gizmos.color = Color.green;
		Gizmos.DrawLine(startPosition, startPosition + transform.forward * distance);

		// Tegn start- og slutpositionerne som sm� kugler
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(startPosition, 0.1f);

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(startPosition + transform.forward * distance, 0.1f);
	}

	void OnCollisionEnter(Collision collision)
	{
		// N�r spilleren r�rer blokken, s�t spilleren som et barn af blokken
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(transform);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		// N�r spilleren forlader blokken, fjern for�ldreskabet
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(null);
		}
	}
}
