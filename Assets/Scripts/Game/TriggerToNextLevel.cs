using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TriggerToNextLevel : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private float secondsTillChangeScene;
    [SerializeField]
    private int sceneIndexToLoad;
    [SerializeField]
    private GameObject fadeScreen;

    private bool startFading = false;
    private float fadeInSpeed = 0.003f;
    float timeToStartFadeIn = 1f;
    private Image fadeScreenImage;

    private void Start()
    {
        // Get fade in screen image
        fadeScreenImage = fadeScreen.GetComponent<Image>();

        // Set fade in screen color to black transparent
        fadeScreenImage.color = new Color(0, 0, 0, 0);
    }

    private void OnTriggerEnter2D()
    {
        // if it is not cutscene, then start cutscene
        if (!player.IsCutscene) {
            player.IsCutscene = true;

            StartCoroutine(StartCutscene());
        }
    }

    private void Update()
    {
        // Smooth fade in to exit the level
        if (startFading) {
            // Increase alpha to make the fadeScreen to fade in
            Color color = fadeScreenImage.color;
            color.a += fadeInSpeed;
            fadeScreenImage.color = color;
        }
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(timeToStartFadeIn);

        startFading = true;

        yield return new WaitForSeconds(secondsTillChangeScene - timeToStartFadeIn);

        // Set camera size and load game scene
        PlayerPrefs.SetFloat("CameraOrthographicSize", 7f);
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
