using UnityEngine;

public class EnterBlock : DefaultClass
{
    private void Start()
    {
        // Disable the block at the game start
        enableChildren(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            enableChildren(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            enableChildren(false);
        }
    }

    private void enableChildren(bool toEnable)
    {
        // Enable or disable every child of this object
        foreach (Transform child in transform) {
            child.gameObject.SetActive(toEnable);
        }
    }
}
