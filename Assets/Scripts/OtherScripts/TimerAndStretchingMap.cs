using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerAndStretchingMap : MonoBehaviour
{
    private bool isMapExpanded = false;
    public Vector3 originalScale;
    public Vector3 tempOriginalScale;
    public Vector3 tempPos;
    public Transform mapTransform;
    ScoreBoard scoreBoard;

    [Header("Timer Variables")]
    public Text timerText;
    private float timeRemaining = 60 * 10;

    public  UnityAction TimerIsFinished;
    // Start is called before the first frame update
    void Start()
    {
        scoreBoard = GetComponent<ScoreBoard>();
        originalScale = mapTransform.localScale;
        tempPos = mapTransform.position;
        tempOriginalScale = originalScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timeRemaining = 0;
            if(timeRemaining == 0)
                scoreBoard.ShowPopUp();
        }
    }

    void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
       
    }

    public void OnMapClicked()
    {
        // Toggle between expanded and original scale
        isMapExpanded = !isMapExpanded;

        if (isMapExpanded)
        {
            // If the map is expanded, set a larger scale (adjust as needed)
            mapTransform.localScale = originalScale * 3.5f;

            // Move the map downward (adjust the Y offset as needed)
            float yOffset = -140.0f; // Adjust this value based on your requirements
            mapTransform.position = new Vector3(mapTransform.position.x+50f, mapTransform.position.y + yOffset, mapTransform.position.z);
        }
        else
        {
            // If the map is not expanded, set the original scale
            mapTransform.localScale = tempOriginalScale;

            // Reset the map's Y position to its original position
            mapTransform.position = tempPos;

        }
    }
}
