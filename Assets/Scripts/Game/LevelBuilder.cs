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
        // Store last block's index for blocks to not repeat
        int lastIndex = 10;

        // Put random blocks to the level
        for (int i = 0; i < blockCount; i++) {
            // Get random index
            int randomBlockIndex = Random.Range(0, blocks.Length);

            // If random index is the same as last index, choose another index
            while (randomBlockIndex == lastIndex) {
                randomBlockIndex = Random.Range(0, blocks.Length);
            }

            // Reset last index
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
