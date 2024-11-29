using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Goal : MonoBehaviour
{
    [Header("Game Objects to Activate")]
    public GameObject[] GOToActivateOnTouch; // Objekter, der aktiveres ved mål
    public GameObject EndScreentoActivate; // Endskærm, der aktiveres
    public float TimeFromGoalToEndscreenDisplay; // Forsinkelse før endskærmen vises

    [Header("Audio Settings")]
    public AudioClip goalSound; // Lydklip, der afspilles ved mål

    private AudioSource audioSource;

    void Start()
    {
        // Find AudioSource-komponenten
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Ingen AudioSource fundet på " + gameObject.name);
        }

        // Deaktiver alle objekter, der skal aktiveres ved mål
        foreach (GameObject g in GOToActivateOnTouch)
        {
            g.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aktiver de angivne GameObjects
            foreach (GameObject g in GOToActivateOnTouch)
            {
                g.SetActive(true);
            }

            // Afspil målets lydklip
            if (goalSound != null)
            {
                audioSource.PlayOneShot(goalSound);
            }

            // Start coroutine for at vise endskærmen
            StartCoroutine(EndScreenDisplay());
        }
    }

    IEnumerator EndScreenDisplay()
    {
        // Vent det angivne antal sekunder
        yield return new WaitForSeconds(TimeFromGoalToEndscreenDisplay);

        // Vis endskærmen
        if (EndScreentoActivate != null)
        {
            EndScreentoActivate.SetActive(true);
        }
    }
}
