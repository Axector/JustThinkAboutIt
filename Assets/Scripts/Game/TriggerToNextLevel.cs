using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class TriggerToNextLevel : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private float secondsTillChangeScene;
    [SerializeField]
    private int sceneIndexToLoad;

    private void OnTriggerEnter2D()
    {
        // if it is not cutscene, then start cutscene
        if (!player.isCutscene) {
            player.isCutscene = true;

            StartCoroutine(StartCutscene());
        }
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(secondsTillChangeScene);

        // Set camera size and load game scene
        PlayerPrefs.SetFloat("CameraOrthographicSize", 7f);
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
