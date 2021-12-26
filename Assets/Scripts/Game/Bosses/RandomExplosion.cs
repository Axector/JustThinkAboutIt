using UnityEngine;

public class RandomExplosion : DefaultClass
{
    [SerializeField]
    private int damage;

    private Popup textPopup;
    private Player player;

    private void Awake()
    {
        textPopup = GetComponent<Popup>();
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            // Deal damage to the player
            other.GetComponent<Player>().AddHealth(-damage);

            // Show damage popup on player
            textPopup.ShowPopup(damage.ToString(), player.transform);
        }
    }
}
