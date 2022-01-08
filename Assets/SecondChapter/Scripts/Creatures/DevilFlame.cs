using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilFlame : MonoBehaviour
{
    [SerializeField]
    private Vector3[] teleportPositions;
    [SerializeField]
    private GameObject minion;
    [SerializeField]
    private Transform parent;

    private int teleportPosintionIndex = 0;
    private Enemy boss;

    private void Start()
    {
        boss = GetComponent<Enemy>();

        // Start teleportation
        StartCoroutine(TeleportDelay());
    }

    private IEnumerator TeleportDelay()
    {
        while (boss.IsAlive) {
            yield return new WaitForSeconds(5f);

            // Spawn boss minions if it is following player
            if (boss.BFollowing) {
                SpawnMinions();
            }

            // Teleport boss to next position 
            transform.localPosition = teleportPositions[teleportPosintionIndex];
            teleportPosintionIndex++;

            // If it is the last position next teleport to the first one
            if (teleportPosintionIndex >= teleportPositions.Length) {
                teleportPosintionIndex = 0;
            }
        }
    }

    private void SpawnMinions()
    {
        // Spawn 4 minions around boss
        for (int i = 0; i < 4; i++) {
            Instantiate(
                minion,
                transform.position + new Vector3(
                    (i == 0) ?  0.16f : (i == 1) ? -0.16f : (i == 2) ? 0.16f : -0.16f,
                    (i == 0) ? -0.16f : (i == 1) ?  0.16f : (i == 2) ? 0.16f : -0.16f,
                    0
                ),
                Quaternion.identity,
                parent
            );
        }
    }
}
