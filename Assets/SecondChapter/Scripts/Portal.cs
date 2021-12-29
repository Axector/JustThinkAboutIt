using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : CollidableObject
{
    [SerializeField]
    private int[] sceneIndexes;
    [SerializeField]
    private GameObject fadingScreen;

    protected override void OnCollision(Collider2D other)
    {
        if (other.name == "Player") {
            // To save money on results page
            PlayerPrefs.SetInt("player_run_money", PlayerPrefs.GetInt("player_run_money", 0) + PlayerPrefs.GetInt("player_money", 0));
            PlayerPrefs.GetInt("save_money", 1);

            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        fadingScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        // Teleport player to the random dungeon room
        int sceneIndex = sceneIndexes[Random.Range(0, sceneIndexes.Length)];
        SceneManager.LoadScene(sceneIndex);
    }
}
