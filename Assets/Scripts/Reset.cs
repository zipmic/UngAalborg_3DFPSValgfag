// Reset Script
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
	public KeyCode resetKey = KeyCode.R;

	void Update()
	{
		if (Input.GetKeyDown(resetKey))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
