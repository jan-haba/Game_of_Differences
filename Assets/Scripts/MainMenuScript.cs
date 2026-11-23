using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainButtonsContainer; 
    public GameObject containerCredits;     
    public GameObject containerControls;   

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CreditButton()
    {
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(false);
        if (containerControls != null) containerControls.SetActive(false);
        if (containerCredits != null) containerCredits.SetActive(true);
    }

    public void ControlsButton()
    {
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(false);
        if (containerCredits != null) containerCredits.SetActive(false);
        if (containerControls != null) containerControls.SetActive(true);
    }

    public void BackToMainButtons()
    {
        if (containerCredits != null) containerCredits.SetActive(false);
        if (containerControls != null) containerControls.SetActive(false);
        if (mainButtonsContainer != null) mainButtonsContainer.SetActive(true);
    }
}