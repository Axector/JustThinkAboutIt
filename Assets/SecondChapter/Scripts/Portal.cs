using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : CollidableObject
{
    [SerializeField]
    private int[] randomSceneIndexes;
    [SerializeField]
    private Vector2Int bossSceneIndexes;      // Number of visited rooms to show boss room | boss room index
    [SerializeField]
    private GameObject fadingScreen;
    [SerializeField]
    private bool bBossRoom;

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

        // Load Results scene after boss fight
        if (bBossRoom) {
            // Get room random inedx to load after Results
            int sceneIndex = randomSceneIndexes[Random.Range(0, randomSceneIndexes.Length)];
            PlayerPrefs.SetInt("next_level", sceneIndex);

            SceneManager.LoadScene(11);
        }

        // DEBUG
        Debug.Log(bossSceneIndexes);
        Debug.Log(PlayerPrefs.GetInt("room_count", 0));

        // Check if next room should be boss room
        if (
            bossSceneIndexes[1] != 0 && 
            bossSceneIndexes[0] == PlayerPrefs.GetInt("room_count", 0)
        ) {
            SceneManager.LoadScene(bossSceneIndexes[1]);
        }
        else { 
            // Teleport player to the random dungeon room
            int sceneIndex = randomSceneIndexes[Random.Range(0, randomSceneIndexes.Length)];
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
