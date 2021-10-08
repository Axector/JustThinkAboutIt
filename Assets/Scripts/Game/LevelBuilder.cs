using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blocks;
    [SerializeField]
    private int blockCount = 10;
    [SerializeField]
    private float distanceBetweenBlocks = 20f;

    private void Awake()
    {
        int lastIndex = 10;

        for (int i = 0; i < blockCount; i++) {
            int randomBlockIndex = Random.Range(0, blocks.Length);

            if (randomBlockIndex == lastIndex) {
                i--;
                continue;
            }

            lastIndex = randomBlockIndex;

            GameObject block = Instantiate(
                blocks[randomBlockIndex],
                transform
            );

            block.transform.localPosition = new Vector3(distanceBetweenBlocks * i, 0, 0);
        }
    }
}
