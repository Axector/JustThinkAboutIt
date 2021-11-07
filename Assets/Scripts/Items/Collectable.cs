using UnityEngine;

public class Collectable : DefaultClass
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy this object when player collects it
        if (other.gameObject.tag == "Player") {
            if (gameObject.tag == "Coin") {
                GetComponent<Money>().Earn();
            }

            Destroy(gameObject);
        }
    }
}
