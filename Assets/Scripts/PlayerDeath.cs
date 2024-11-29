using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    public Behaviour[] BehavioursToDisableOnDeath; // Komponenter, der deaktiveres ved død
    public AudioClip deathSound; // Lyden, der afspilles ved død
    public Image deathScreen; // Referencer til det røde canvas-image
    public float fadeDuration = 1f; // Varighed for fade-effekt

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Hent AudioSource på spilleren
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Ingen AudioSource fundet på " + gameObject.name + ". Tilføj en AudioSource.");
        }

        if (deathScreen != null)
        {
            // Sørg for, at skærmen starter disabled
            deathScreen.enabled = false;
        }
    }

    public void KillPlayer()
    {
        // Deaktiver de angivne komponenter
        foreach (Behaviour b in BehavioursToDisableOnDeath)
        {
            b.enabled = false;
        }

        // Tilføj en tilfældig rotation
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 5);

        // Afspil dødslyden
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Start fade-effekten
        if (deathScreen != null)
        {
            StartCoroutine(FadeOutScreen());
        }
    }

    private IEnumerator FadeOutScreen()
    {
        float timer = 0f;

        // Enable skærmen og gør den helt rød
        deathScreen.enabled = true;
        Color color = deathScreen.color;
        color.a = 1f;
        deathScreen.color = color;

        // Fad ud over tid
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            deathScreen.color = color;
            yield return null;
        }

        // Sørg for, at den er helt gennemsigtig til sidst
        color.a = 0f;
        deathScreen.color = color;

        // Disable skærmen igen
        deathScreen.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KillPlayer();
        }
    }
}
