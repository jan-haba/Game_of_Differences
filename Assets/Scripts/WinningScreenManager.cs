using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScreenManager : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }
}
