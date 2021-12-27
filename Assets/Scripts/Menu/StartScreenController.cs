using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class StartScreenController : DefaultClass
{
    [SerializeField]
    private float delayBeforeNextScene;
    [SerializeField]
    private int nextScene;
    [SerializeField]
    private Text time;
    [SerializeField]
    private Animator title;

    private void Start()
    {
        StartCoroutine(WaitToEndScene());
    }

    private IEnumerator WaitToEndScene()
    {
        yield return new WaitForSeconds(delayBeforeNextScene);

        time.text = PlayerPrefs.GetString("total_time", "00:00:00");

        yield return new WaitForSeconds(3f);

        title.Play("Small Title");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(nextScene);
    }
}
