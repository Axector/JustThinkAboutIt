using System.Collections;
using UnityEngine;

public class Enemy_Patroling_FlyingFollowing_Shooter : Enemy_Patroling
{
    [SerializeField]
    private float distanceFromPlayer;
    [SerializeField]
    private float playerLoseDistance = 5f;
    [SerializeField]
    private float returnBackDistance = 10f;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private ParticleSystem spriteParticleSystem;
    [SerializeField]
    private float deathSpeed;
    [SerializeField]
    private AudioClip shotAudio;
    [SerializeField]
    private ParticleSystem selfExplosion;

    private bool attack = false;

    public bool getAttack { get => attack; }

    protected override void Update()
    {
        base.Update();

        if (attack) {
            // Get distance to player
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float startDistance = Vector3.Distance(transform.position, startingPosition);
            
            // If player is far from the enemy
            if (distance >= playerLoseDistance || startDistance >= returnBackDistance) {
                attack = false;
            }
        }

        // Stop attack player after his death
        if (!player.IsAlive) {
            attack = false;
        }
    }

    protected override void FixedUpdate()
    {
        // Patroling or attacking
        if (attack) {
            Attack();
        }
        else {
            Patrol();
        }

        // Destroy enemy if it is dead
        if (!isAlive) {
            // Get value to decrese enemy size
            Vector3 scaleDecrease = deathSpeed * Vector3.one * Time.fixedDeltaTime;

            // Get scale values
            float spriteScaleX = spriteRenderer.transform.localScale.x;
            float particlesScaleX = spriteParticleSystem.transform.localScale.x;

            // Decrease both sprite and particles sizes till 0
            spriteRenderer.transform.localScale -= (spriteScaleX > 0)
                ? scaleDecrease
                : Vector3.zero;
            spriteParticleSystem.transform.localScale -= (particlesScaleX > 0)
                ? scaleDecrease
                : Vector3.zero;

            // Get scale values to know which to compare
            spriteScaleX = spriteRenderer.transform.localScale.x;
            particlesScaleX = spriteParticleSystem.transform.localScale.x;

            // Check if an enemy is small enough to be destroyed
            if (Mathf.Max(spriteScaleX, particlesScaleX) <= 0) {
                DestroyEnemy();
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D other) {}

    public void StartAttack()
    {
        attack = true;

        // Take a look at the player before attack
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        transform.rotation = GetLookAtRotation(lookDirection);

        // Start shooting
        StartCoroutine(Shot());
    }

    private void Attack()
    {
        // Find the direction to the place above player
        Vector3 direction = ((player.transform.position + Vector3.up * distanceFromPlayer) - transform.position).normalized;

        // Look at the player
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        transform.rotation = GetLookAtRotation(lookDirection);

        // Stay above player
        rigidBody2D.MovePosition(transform.position + direction * speed * speedIncrease * Time.fixedDeltaTime);
    }

    private IEnumerator Shot()
    {
        while (attack) {
            // Play shot sound
            PlaySound(audioSource, shotAudio);

            // Create bullet
            GameObject newBullet = Instantiate(
                bullet,
                transform.position,
                transform.rotation
            );

            // Call bullet constructor
            Bullet_Standard standardBullet;
            if (newBullet.TryGetComponent<Bullet_Standard>(out standardBullet)) {
                standardBullet.Setup(this);
            }

            // Delay before next shot
            yield return new WaitForSeconds(delayBeforeAttack);
        }
    }

    public override void SetHealth(int hp)
    {
        base.SetHealth(hp);

        if (!isAlive) {
            Instantiate(
                selfExplosion,
                transform.position,
                Quaternion.identity,
                transform
            );
        }

        // Agression after taking damage
        if (!attack) {
            StartAttack();
        }
    }
}
