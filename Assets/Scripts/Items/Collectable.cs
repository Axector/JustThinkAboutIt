using UnityEngine;

public class Collectable : DefaultClass
{
    [SerializeField]
    private AudioClip collectSound;

    private AudioSource audioSource;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy this object when player collects it
        if (other.gameObject.tag == "Player") {
            if (gameObject.tag == "Coin") {
                GetComponent<Money>().Earn();
            }

            // Play collection sound
            PlaySound(player.AudioSource, collectSound);

            Destroy(gameObject);
        }
    }
}
