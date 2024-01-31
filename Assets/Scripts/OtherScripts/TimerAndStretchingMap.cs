using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerAndStretchingMap : MonoBehaviour
{
    private bool isMapExpanded = false;
    public Vector3 originalScale;
    public Vector3 temporiginalScale;
    public Transform mapTransorm;

    [Header("Timer Variables")]
    public Text timerText;
    private float timeRemaining = 60 * 10;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = mapTransorm.localScale;
    }

    // Update is called once per frame
    void Update()
    {
       if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timeRemaining = 0;  
        }
    }

    void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
    public void OnMapClicked()
    {// Toggle between expanded and original scale
        isMapExpanded = !isMapExpanded;

        if (isMapExpanded)
        {
            temporiginalScale = mapTransorm.localScale;
            // If the map is expanded, set a larger scale (adjust as needed)
            mapTransorm.localScale = originalScale * 3.5f;
        }
        else
        {
            // If the map is not expanded, set the original scale
            mapTransorm.localScale = temporiginalScale;
        }
    }
}
