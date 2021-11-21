using System.Collections;
using UnityEngine;

public class Enemy_Fireball : DefaultClass
{
    [SerializeField]
    private ParticleSystem explosion;

    private Player player;
    private AudioSource audioSource;
    protected Enemy_Ground_Shooter parent;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
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
            PlaySound(parent.AudioSource, player.playerAttackExplosionSound);

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
