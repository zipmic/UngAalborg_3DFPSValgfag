using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
	public enum RotationAxis
	{
		X, // Roter omkring x-aksen
		Y, // Roter omkring y-aksen
		Z  // Roter omkring z-aksen
	}

	[Header("Rotation Settings")]
	public RotationAxis rotationAxis = RotationAxis.Y; // Standardakse
	public float rotationSpeed = 10f; // Hastighed for rotationen

	void Update()
	{
		// V�lg akse baseret p� enum og roter omkring den
		switch (rotationAxis)
		{
			case RotationAxis.X:
				transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
				break;
			case RotationAxis.Y:
				transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
				break;
			case RotationAxis.Z:
				transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
				break;
		}
	}
}
