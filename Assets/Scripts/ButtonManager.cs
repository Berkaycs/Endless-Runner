using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager.instance.PlayClickButton();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayClickButton();
        Application.Quit();
    }

    public void RestartGame()
    {
        AudioManager.instance.PlayClickButton();
        SceneManager.LoadScene(1);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
