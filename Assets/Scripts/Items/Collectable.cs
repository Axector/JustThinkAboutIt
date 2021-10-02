using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy this object when player collects it
        if (other.gameObject.tag == "Player") {
            if (gameObject.tag == "Coin") {
                GetComponent<Money>().earn();
            }

            Destroy(gameObject);
        }
    }
}
