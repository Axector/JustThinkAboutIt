using UnityEngine;

public class Collectable : DefaultClass
{
    [SerializeField]
    private AudioClip collectSound;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy this object when player collects it if it is collectable
        if (other.gameObject.tag == "Player" && DoActionByTag()) {
            // Play collection sound
            PlaySound(player.AudioSource, collectSound);

            Destroy(gameObject);
        }
    }

    private bool DoActionByTag()
    {
        // Collect coins
        if (gameObject.tag == "Coin") {
            GetComponent<Money>().Earn();

            return true;
        }

        // Collect health points if player does not have maximum health points
        if (gameObject.tag == "HealthItem" && player.Health != player.MaxHealth) {
            GetComponent<HealthItem>().IncreasePlayerHealth(player);

            return true;
        }

        return false;
    }
}
