using UnityEngine;

public class DetectEnemy_Fireball : DetectEnemy
{
    [SerializeField]
    private float fallSpeed;
    [SerializeField]
    private ParticleSystem explosion;

    private Rigidbody2D rigidBody2D;

    protected override void Awake()
    {
        base.Awake();

        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Fall down
        MoveDown();
    }
    
    private void MoveDown()
    {
        rigidBody2D.velocity += Vector2.down * fallSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;

        // Explode on collision
        if (otherTag == "Enemy" || otherTag == "SolidBlock") {
            // Play explosion sound
            player.PlaySound(player.playerAttackExplosionSound);

            // Play explosion
            Instantiate(
                 explosion,
                 transform.position,
                 Quaternion.identity
            );

            // Deal damage to an enemy
            if (otherTag == "Enemy") {
                other.GetComponent<AEnemy>().SetHealth(-player.Damage * 2);
                textPopup.ShowPopup((player.Damage * 2).ToString(), other.gameObject.transform);
            }

            Destroy(gameObject);
        }
    }
}
