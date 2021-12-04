using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class Titles : MonoBehaviour
{
    [SerializeField]
    private GameObject fadingScreenIn;

    private void Start()
    {
        StartCoroutine(StartTitles());
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

        SceneManager.LoadScene(0);
    }
}
