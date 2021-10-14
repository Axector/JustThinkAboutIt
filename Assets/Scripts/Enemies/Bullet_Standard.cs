using UnityEngine;

public class Bullet_Standard : ABullet
{
    [SerializeField]
    private ParticleSystem explosion;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Get tag of other
        string tag = other.gameObject.tag;

        // Destroy the bullet when it hits something
        if (tag == "SolidBlock") {
            Explode();
            Destroy(gameObject);
        }

        if (tag == "Player") {
            // Deal damage to the player
            parent.DoDamage();

            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        // Create explosion
        Instantiate(
            explosion,
            transform.position,
            Quaternion.identity
        );
    }
}
