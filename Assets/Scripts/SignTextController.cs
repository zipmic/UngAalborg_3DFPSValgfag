using UnityEngine;
using TMPro;

[ExecuteAlways] // Gør, at scriptet kører i edit-mode
public class SignTextController : MonoBehaviour
{
	[Header("Sign Settings")]
	[TextArea] public string signText = "Skriv din tekst her"; // Tekst, som skal vises på skiltet

	private TextMeshPro textMeshPro;

	void Awake()
	{
		// Find TextMeshPro-komponenten i child-objekterne
		textMeshPro = GetComponentInChildren<TextMeshPro>();

		if (textMeshPro == null)
		{
			Debug.LogError("Ingen TextMeshPro-komponent fundet som child af " + gameObject.name);
		}
	}

	void Update()
	{
		// Kun opdatér teksten, hvis scriptet kører i editor eller spillet er aktivt
		if (!Application.isPlaying)
		{
			UpdateSignText();
		}
	}

	/// <summary>
	/// Opdaterer teksten i TextMeshPro-komponenten.
	/// </summary>
	public void UpdateSignText()
	{
		if (textMeshPro != null && textMeshPro.text != signText)
		{
			textMeshPro.text = signText;
		}
	}
}
