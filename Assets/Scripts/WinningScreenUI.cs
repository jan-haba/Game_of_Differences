using UnityEngine;
using TMPro;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TextMeshProUGUI wrongClicksText;

    private void Start()
    {
        if (finalScoreText != null)
            finalScoreText.text = $"Score: {GameManager.FinalScore}";

        if (finalTimeText != null)
            finalTimeText.text = $"Time: {GameManager.FinalTime}";

        if (wrongClicksText != null)
            wrongClicksText.text = $"Wrong Clicks: {GameManager.WrongClicks}";
    }
}