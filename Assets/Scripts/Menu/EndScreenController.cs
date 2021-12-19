using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class EndScreenController : DefaultClass
{
    [SerializeField]
    private float delayBeforeExit;
    [SerializeField]
    private Text timeText;

    private void Awake()
    {
        StartCoroutine(ShowTimeSpent());
    }

    private IEnumerator ShowTimeSpent()
    {
        yield return new WaitForSeconds(delayBeforeExit);

        timeText.text = PlayerPrefs.GetString("total_time");
        yield return new WaitForSeconds(4f);

        Application.Quit();

        // DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
