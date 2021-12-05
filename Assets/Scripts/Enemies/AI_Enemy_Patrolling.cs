using System.Collections;
using UnityEngine;

public class AI_Enemy_Patrolling : AEnemy
{
    [SerializeField]
    private float rayDistance = 0.05f;
    [SerializeField]
    private float rayOffsetDown = 0.75f;
    [SerializeField]
    private float rayOffsetRight = 0.3f;
    [SerializeField]
    private float delayBeforeMove = 1f;
    [SerializeField]
    private float detectDistance = 7f;
    [SerializeField]
    private float attackDistance = 0.5f;
    [SerializeField]
    private float distanceToGoBack = 15f;
    [SerializeField]
    private float jumpForce = 7f;
    [SerializeField]
    private AudioClip[] attackSounds;

    private bool seeGround = false;
    private bool seePlayer = false;
    private bool isFollowing = false;
    private bool isGoingBack = false;
    private bool doAttack = false;
    private bool isWaiting = false;

    private void Update()
    {
        // Check if an enemy is seeing the ground
        CheckGround();

        // Check if an enemy can detect the player
        seePlayer = (isFollowing) ? true : CheckPlayer();

        // Change animation if an enemy is moving or not
        animator.SetBool("isMoving", !isWaiting);

        if (isFollowing) {
            CheckFollowing();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D other) { }

    private void FixedUpdate()
    {
        if (isAlive) {
            if (!seePlayer && !isGoingBack) {
                // Patrolling
                Patrolling();
            }
            else if (seePlayer && !doAttack && !isGoingBack) {
                // Go and attack player
                MoveToPlayer();
            }
            else if (isGoingBack) {
                // Go back to start position
                CheckGoBack();
            }
        }
    }

    private void CheckGoBack()
    {
        // If x coordinates are nearly equal
        if (NearlyEqual(transform.position.x, startingPosition.x, 0.1f)) {
            // If y coordinates are nearly equal
            if (NearlyEqual(transform.position.y, startingPosition.y, 0.2f)) { 
                isGoingBack = false;
                collider2d.isTrigger = false;
                rigidBody2D.gravityScale = 1;
            }
            // If an enemy is lower than starting position
            else if (transform.position.y < startingPosition.y) {
                collider2d.isTrigger = true;
                rigidBody2D.gravityScale = 0;

                // Go up
                transform.position += new Vector3(0, speed * Time.fixedDeltaTime, 0);
            }
            // If an enemy is higher than starting position
            else if (transform.position.y > startingPosition.y) {
                collider2d.isTrigger = true;
            }
        }
        else {
            GoToPosition(startingPosition);
        }
    }

    private void CheckFollowing()
    {
        float distanceToStart = Vector2.Distance(transform.position, startingPosition);
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Stop following if the enemy is too far from starting position
        if (distanceToStart > distanceToGoBack) {
            isGoingBack = true;
            isFollowing = false;
        }

        // Attack if near enough to the player
        if (distance <= attackDistance && !doAttack) {
            // Flip sprite horizontaly to face player
            spriteRenderer.flipX = (player.transform.position.x <= transform.position.x);

            StartCoroutine(DoAttack());
        }
    }

    private void Patrolling()
    {
        // Stop when enemy does not see ground in front of it
        if (seeGround && !isWaiting) {
            // Move untill enemy sees the end of a platform
            transform.position += new Vector3(((spriteRenderer.flipX) ? -speed : speed) * Time.fixedDeltaTime, 0, 0);
        }
        else if (!isWaiting) {
            // Wait before next move
            isWaiting = true;
            StartCoroutine(WaitBeforeMove());
        }
    }

    private void CheckDoDamage()
    {
        // Create ray to check if player is in front of the enemy
        LayerMask mask = LayerMask.GetMask("Floor", "Player");
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            transform.position,
            (spriteRenderer.flipX) ? Vector2.left : Vector2.right,
            1.5f,
            mask
        );

        // DEBUG
        Debug.DrawRay(transform.position, (spriteRenderer.flipX) ? Vector2.left : Vector2.right);

        // Get random punch sound
        AudioClip punchSound = attackSounds[Random.Range(0, attackSounds.Length)];

        // Play punch sound
        PlaySound(audioSource, punchSound);

        // Check if ray hits anything
        if (hits.Length > 0) {
            // If player is on the attack track do damage
            if (hits[0].collider.gameObject.tag == "Player") {
                DoDamage();
            }
        }
    }

    private IEnumerator DoAttack()
    {
        doAttack = true;
        isWaiting = true;

        animator.Play("Cyborg_Attack");

        yield return new WaitForSeconds(delayBeforeAttack * 3);

        isWaiting = false;
        doAttack = false;
    }

    private void MoveToPlayer()
    {
        // Get the player position and distance to it
        Vector3 playerPosition = player.transform.position;
        
        // Start following
        isFollowing = true;

        GoToPosition(playerPosition);

        // If player is higher than an enemy, do jump
        if (playerPosition.y > transform.position.y + 1.5f && seeGround) {
            animator.Play("Cyborg_Jump");

            rigidBody2D.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );
        }
    }

    private void GoToPosition(Vector3 position)
    {
        // If player is on the left side from an enemy
        if (position.x <= transform.position.x) {
            transform.position += new Vector3(-speed * Time.fixedDeltaTime, 0, 0);

            // If an enemy looks to the right
            if (!spriteRenderer.flipX) {
                HorizontalFlip();
            }
        }
        // If player is on the right side from an enemy
        else {
            transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);

            // If an enemy looks to the left
            if (spriteRenderer.flipX) {
                HorizontalFlip();
            }
        }
    }

    private bool CheckPlayer()
    {
        Vector3 directon = (player.transform.position - transform.position);

        // Check if an enemy has eye contact with player
        LayerMask mask = LayerMask.GetMask("Floor", "Player");
        //Vector3 directon = (player.transform.position - transform.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directon, detectDistance + 10f, mask);

        // Check if ray hits anything
        if (hits.Length <= 0) {
            return false;
        }

        // Get hitted game object
        GameObject hitObj = hits[0].collider.gameObject;

        // if it hits Player
        if (hitObj.tag == "Player") {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // If an enemy can detect this player
            return distance <= detectDistance;
        }

        return false;
    }

    private void CheckGround()
    {
        // Get Floor layer
        LayerMask mask = LayerMask.GetMask("Floor");

        // DEBUG
        Debug.DrawRay(new Vector2(transform.position.x + rayOffsetRight, transform.position.y - rayOffsetDown), Vector2.down * rayDistance, Color.cyan);
        Debug.DrawRay(new Vector2(transform.position.x + rayOffsetRight, transform.position.y - rayOffsetDown), Vector2.down * rayDistance, Color.cyan);

        // Cast ray to check if there is the ground in front of an enemy
        RaycastHit2D[] rayHit = Physics2D.RaycastAll(
                new Vector2(transform.position.x + rayOffsetRight, transform.position.y - rayOffsetDown),
                Vector2.down,
                rayDistance,
                mask
            );

        // Change isGrounded value if ray hit any Floor
        seeGround = rayHit.Length > 0;
        animator.SetBool("isGrounded", seeGround);
    }

    private void HorizontalFlip()
    {
        // Flip sprite and ground checking ray
        spriteRenderer.flipX = !spriteRenderer.flipX;
        rayOffsetRight *= -1;
    }

    private IEnumerator WaitBeforeMove()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        HorizontalFlip();
        isWaiting = false;
    }
}
