using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = true;

    public string FormattedTime => TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss\.ff");

    private void Update()
    {
        if (!isRunning || Time.timeScale == 0f) return;

        elapsedTime += Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = FormattedTime;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }
}