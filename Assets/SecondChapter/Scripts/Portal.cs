using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : CollidableObject
{
    [SerializeField]
    private int[] sceneIndexes;

    protected override void OnCollision(Collider2D other)
    {
        if (other.name == "Player") {
            // Teleport player to the random dungeon room
            int sceneIndex = sceneIndexes[Random.Range(0, sceneIndexes.Length)];

            // To save money on results page
            PlayerPrefs.GetInt("save_money", 1);

            // Load that randpm scene
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
