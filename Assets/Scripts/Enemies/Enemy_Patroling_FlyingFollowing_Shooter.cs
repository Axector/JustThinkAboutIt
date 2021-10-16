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

    private bool attack = false;

    protected override void Update()
    {
        base.Update();

        if (attack) {
            // Get distance to player
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float startDistance = Vector3.Distance(transform.position, startingPosition);

            // DEBUG
            Debug.DrawRay(transform.position, player.transform.position - transform.position, (distance >= playerLoseDistance) ? Color.red : Color.green);
            Debug.DrawRay(transform.position, startingPosition - transform.position, Color.yellow);
            
            // If player is far from the enemy
            if (distance >= playerLoseDistance || startDistance >= returnBackDistance) {
                attack = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;

        // Atack player if he is in attack area
        if (otherTag == "Player" && !attack) {
            StartAttack();
        }
    }

    protected override void FixedUpdate()
    {
        // Patroling or attacking
        if (attack) {
            collider2d.isTrigger = false;

            Attack();
        }
        else {
            // The enemy can go through the bojects, when not attacking the player and vice versa
            collider2d.isTrigger = true;

            Patrol();
        }
    }

    private void StartAttack()
    {
        attack = true;

        // Take a look at the player before attack
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        LookAtDirection(lookDirection);

        // Start shooting
        StartCoroutine(Shot());
    }

    private void Attack()
    {
        // Find the direction to the place above player
        Vector3 direction = ((player.transform.position + Vector3.up * distanceFromPlayer) - transform.position).normalized;

        // Look at the player
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        LookAtDirection(lookDirection);

        // Stay above player
        rigidBody2D.MovePosition(transform.position + direction * speed * speedIncrease * Time.fixedDeltaTime);
    }

    private IEnumerator Shot()
    {
        while (attack) {
            // Create bullet
            GameObject newBullet = Instantiate(
                bullet,
                transform.position,
                transform.rotation
            );

            // Call bullet constructor
            newBullet.GetComponent<Bullet_Standard>().Setup(this);

            // Delay before next shot
            yield return new WaitForSeconds(delayBeforeAttack);
        }
    }

    public override void SetHealth(int hp)
    {
        base.SetHealth(hp);

        // Agression after taking damage
        if (!attack) {
            StartAttack();
        }
    }
}
