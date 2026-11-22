using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int FinalScore { get; private set; }
    public static string FinalTime { get; private set; }
    public static int WrongClicks { get; private set; }

    [SerializeField] private int totalDifferences = 5;
    [SerializeField] private string winSceneName = "WinScene";

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Timer gameTimer; 

    private int currentScore = 0;
    private int foundDifferences = 0;
    private bool isGameFinished = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentScore = 0;
        foundDifferences = 0;
        WrongClicks = 0;
        isGameFinished = false;

        if (gameTimer != null)
            gameTimer.ResetTimer();

        UpdateScoreUI();
    }

    public void AddScore()
    {
        if (isGameFinished) return;

        currentScore++;
        foundDifferences++;
        UpdateScoreUI();

        if (foundDifferences >= totalDifferences)
        {
            WinGame();
        }
    }

    public void SubtractScore()
    {
        if (isGameFinished) return;

        currentScore = Mathf.Max(0, currentScore - 1);
        WrongClicks++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Skóre: {currentScore}";
        }
    }

    private void WinGame()
    {
        isGameFinished = true;
        FinalScore = currentScore;

        if (gameTimer != null)
        {
            gameTimer.StopTimer();
            FinalTime = gameTimer.FormattedTime;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(winSceneName);
    }
}