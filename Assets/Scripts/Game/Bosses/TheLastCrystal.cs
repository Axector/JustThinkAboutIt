using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class TheLastCrystal : DefaultClass
{
    [SerializeField]
    private GameObject playerController;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private Animator cutsceneCamera;
    [SerializeField]
    private int nextLevelIndex;
    [SerializeField]
    private KeyObjectDestroy keyObject;
    [SerializeField]
    private GameObject fadingScreen;
    [SerializeField]
    private GameObject fairyToSpawn;

    private bool bToggle = true;

    private void Start()
    {
        // Set scene color
        PostProcessVolume postProcessing = cutsceneCamera.GetComponent<PostProcessVolume>();
        ColorGrading colorGrading;
        postProcessing.profile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = cameraController.SceneColor;

        // Start cutscene
        StartCoroutine(StartScene());

        PlayerPrefs.SetInt("next_level", nextLevelIndex);
    }

    private void Update()
    {
        if (keyObject && bToggle && keyObject.IsDestroyed) {
            bToggle = false;

            // Spawn fairy if it is first time player managed to beat the first chapter
            if (PlayerPrefs.GetInt("first_chapter_beaten", 0) == 0) {
                StartCoroutine(EndAfterDialog());
            }
            else { 
                StartCoroutine(EndLastLevel());
            }
        }
    }

    private IEnumerator EndAfterDialog()
    {
        fairyToSpawn.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(62f);

        PlayerPrefs.SetInt("first_chapter_beaten", 1);

        StartCoroutine(EndLastLevel());
    }

    private IEnumerator StartScene()
    {
        yield return new WaitForSeconds(0.1f);

        // Stop player controller
        playerController.SetActive(false);

        yield return new WaitForSeconds(1.9f);

        // Show the boss
        cameraController.SelectNextCamera();
        cutsceneCamera.Play("EnterTheLastCrystalScene");

        yield return new WaitForSeconds(7.5f);

        // Resume the game
        cameraController.SelectNextCamera();
        playerController.SetActive(true);

        Destroy(cutsceneCamera.gameObject);
    }

    private IEnumerator EndLastLevel()
    {
        yield return new WaitForSeconds(4f);

        fadingScreen.SetActive(true);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(11);
    }
}
