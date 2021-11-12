using UnityEngine;

public class Enemy_Fireball : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosion;

    private Player player;
    protected Enemy_Ground_Shooter parent;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void Setup(Enemy_Ground_Shooter parent)
    {
        this.parent = parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;

        // Explode on collision
        if (otherTag == "Player" || otherTag == "SolidBlock") {
            // Play explosion sound
            player.AudioSource.clip = player.playerAttackExplosionSound;
            player.AudioSource.Play();

            // Play explosion
            Instantiate(
                 explosion,
                 transform.position,
                 Quaternion.identity
            );

            // Deal damage to an enemy
            if (otherTag == "Player") {
                parent.DoDamage();
            }

            Destroy(gameObject);
        }
    }
}
