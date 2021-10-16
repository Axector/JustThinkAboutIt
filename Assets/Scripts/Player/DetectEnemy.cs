using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    protected Player player;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Deal damage to an enemy
        if (other.gameObject.tag == "Enemy") {
            other.GetComponent<AEnemy>().SetHealth(-player.Damage);
            Destroy(gameObject);
        }
    }
}
