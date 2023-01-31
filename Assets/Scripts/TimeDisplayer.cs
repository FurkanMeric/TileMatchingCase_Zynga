using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro timeDisplay;
   
    void Start()
    {
        GameManager.OnTimerChanged += UpdateTimer;
    }

    
    private void UpdateTimer(int timer)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        timeDisplay.text = timeSpan.Minutes + ":" + timeSpan.Seconds;
    }
}
