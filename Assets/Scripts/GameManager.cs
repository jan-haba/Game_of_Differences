using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int FinalScore { get; private set; }
    public static string FinalTime { get; private set; }
    public static int WrongClicks { get; private set; }

    [SerializeField] private int totalDifferences = 5;
    [SerializeField] private string winSceneName = "WinScene";

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI differencesText;
    [SerializeField] private Timer gameTimer;

    [SerializeField] private float hintDuration = 0.5f;

    private InputAction hintInput;
    private int currentScore = 0;
    private int foundDifferences = 0;
    private bool isGameFinished = false;
    private bool isHintActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        currentScore = 0;
        foundDifferences = 0;
        WrongClicks = 0;
        isGameFinished = false;

        hintInput = InputSystem.actions.FindAction("Hint");
        if (hintInput != null)
        {
            hintInput.started += OnHintPressed;
        }

        if (gameTimer != null)
            gameTimer.ResetTimer();

        UpdateScoreUI();
    }

    private void OnDestroy()
    {
        if (hintInput != null)
        {
            hintInput.started -= OnHintPressed;
        }
    }

    private void OnHintPressed(InputAction.CallbackContext ctx)
    {
        if (Time.timeScale == 0f || isGameFinished || isHintActive) return;
        StartCoroutine(ShowHintCoroutine(hintDuration));
    }

    private IEnumerator ShowHintCoroutine(float duration)
    {
        isHintActive = true;
        DifferenceObject[] allDifferences = FindObjectsByType<DifferenceObject>(FindObjectsSortMode.None);

        List<DifferenceObject> remainingDifferences = new List<DifferenceObject>();
        foreach (var diff in allDifferences)
        {
            if (!diff.IsFixed)
            {
                remainingDifferences.Add(diff);
            }
        }

        if (remainingDifferences.Count > 0)
        {
            DifferenceObject randomDiff = remainingDifferences[Random.Range(0, remainingDifferences.Count)];
            randomDiff.ShowHint(true);

            yield return new WaitForSeconds(duration);

            if (!randomDiff.IsFixed)
            {
                randomDiff.ShowHint(false);
            }
        }

        isHintActive = false;
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
            scoreText.text = $"Score: {currentScore}";

        if (differencesText != null)
            differencesText.text = $"Differences: {foundDifferences} / {totalDifferences}";
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