using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public float StartTime = 60;
    public TextMeshProUGUI TextForDisplayingTimer;
    public string TextPrefix = "Time left: ";
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        timer = StartTime;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(timer >= 0)
        {
            timer -= Time.deltaTime;

            TextForDisplayingTimer.text = TextPrefix + timer.ToString("F1");
        }
        else
        {
            timer = 0;
            TextForDisplayingTimer.text = TextPrefix + timer.ToString("F1");
            EndGame();
        }
    }

    private void EndGame()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerDeath>().KillPlayer();
    }

    public void ChangeTime(float timeChange)
    {
        timer += timeChange;
    }
}
