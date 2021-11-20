using UnityEngine.SceneManagement;
using UnityEngine;

public class QuickMenu : DefaultClass
{
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {

    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
