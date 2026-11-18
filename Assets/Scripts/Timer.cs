using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timerText.text = TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss\.ff");
    }
}
