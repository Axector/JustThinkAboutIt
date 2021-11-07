using UnityEngine;

public class LevelBuilder : DefaultClass
{
    [SerializeField]
    private GameObject[] blocks;
    [SerializeField]
    private int blockCount;
    [SerializeField]
    private float distanceBetweenBlocks;

    private void Awake()
    {
        // Store last block's index for blocks to not repeat
        int lastIndex = blockCount;
        int prelastIndex = lastIndex;

        // Put random blocks to the level
        for (int i = 0; i < blockCount; i++) {
            // Get random index
            int randomBlockIndex = Random.Range(0, blocks.Length);

            // If random index is the same as last and pre-last index, choose another index
            while (randomBlockIndex == lastIndex || randomBlockIndex == prelastIndex) {
                randomBlockIndex = Random.Range(0, blocks.Length);
            }

            // Set new last and pre-last indexes
            prelastIndex = lastIndex;
            lastIndex = randomBlockIndex;

            // Block addition to the level
            Instantiate(
                blocks[randomBlockIndex],
                new Vector3(distanceBetweenBlocks * i + 15, 0, -0.5f),
                Quaternion.identity,
                transform
            );
        }
    }
}
