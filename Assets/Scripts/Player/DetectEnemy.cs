using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Deal damage to an enemy
        if (other.gameObject.tag == "Enemy") {
            other.GetComponent<AEnemy>().SetHealth(-player.Damage / 2);
            Destroy(gameObject);
        }
    }
}
