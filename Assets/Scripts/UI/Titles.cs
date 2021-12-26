using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class Titles : DefaultClass
{
    [SerializeField]
    private GameObject fadingScreenIn;

    private void Start()
    {
        StartCoroutine(StartTitles());

        // Second chapter can be opened
        PlayerPrefs.SetInt("open_second_chapter", 1);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            StartCoroutine(EndScene());
        }
    }

    private IEnumerator StartTitles()
    {
        yield return new WaitForSeconds(30f);

        StartCoroutine(EndScene());
    }

    private IEnumerator EndScene()
    {
        fadingScreenIn.SetActive(true);

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(1);
    }
}
