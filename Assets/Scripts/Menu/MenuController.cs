using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class MenuController : DefaultClass
{
    public void StartFirstChapter()
    {
        SceneManager.LoadScene(1);
    }

    public void StartSecondChapter()
    {
        SceneManager.LoadScene(0);
    }

    public void EnterSettings()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();

        // DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
