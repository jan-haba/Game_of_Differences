using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    public UnityEvent OnScoredChanged;
    public int Score { get; private set; }

    public void AddScore(int amount) 
    {
        Score += amount;
        OnScoredChanged.Invoke();
    }
}
