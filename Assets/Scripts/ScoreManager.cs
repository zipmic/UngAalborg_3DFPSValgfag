using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private int currentCoins = 0;

    [SerializeField] private AudioSource _scoreManagerAudio;


    private void Start()
    {
        RefreshUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: "+currentCoins.ToString();
        }
    }

    public void PlayPickUpSound(AudioClip SFX)
    {
        _scoreManagerAudio.PlayOneShot(SFX);
    }
}
