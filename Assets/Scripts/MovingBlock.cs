using UnityEngine;

public class MovingBlock : MonoBehaviour
{
	[Header("Movement Settings")]
	public float speed = 2f; // Hastighed på bevægelsen
	public float distance = 5f; // Afstanden, blokken bevæger sig

	private Vector3 startPosition; // Startpositionen for blokken
	private Vector3 targetPosition; // Slutpositionen for blokken
	private bool movingForward = true; // Om blokken bevæger sig fremad

	void Start()
	{
		// Gem startpositionen og beregn slutpositionen
		startPosition = transform.position;
		targetPosition = startPosition + transform.forward * distance;
	}

	void Update()
	{
		// Bevæg blokken frem og tilbage
		if (movingForward)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

			// Skift retning, hvis den når slutpositionen
			if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
			{
				movingForward = false;
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

			// Skift retning, hvis den når startpositionen
			if (Vector3.Distance(transform.position, startPosition) < 0.01f)
			{
				movingForward = true;
			}
		}
	}

	void OnDrawGizmos()
	{
		// Tegn en linje, der viser bevægelsen mellem start og slut
		Gizmos.color = Color.green;
		Gizmos.DrawLine(startPosition, startPosition + transform.forward * distance);

		// Tegn start- og slutpositionerne som små kugler
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(startPosition, 0.1f);

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(startPosition + transform.forward * distance, 0.1f);
	}

	void OnCollisionEnter(Collision collision)
	{
		// Når spilleren rører blokken, sæt spilleren som et barn af blokken
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(transform);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		// Når spilleren forlader blokken, fjern forældreskabet
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.transform.SetParent(null);
		}
	}
}
