using System.Collections;
using UnityEngine;

public class SmipleEnemy_2 : AEnemy_2
{
    [SerializeField]
    private float delayBeforeMove = 1f;
    [SerializeField]
    private float distanceFromPlayer;
    [SerializeField]
    private float playerLoseDistance = 5f;
    [SerializeField]
    private Vector3 leftPosition;
    [SerializeField]
    private Vector3 rightPosition;

    private bool moveRight   = true;
    private bool moveLeft    = false;
    private bool attack      = false;

    private void Update()
    {
        // DEBUG
        Debug.DrawRay(transform.position, player.transform.position + Vector3.up * distanceFromPlayer - transform.position, Color.blue);

        // Stop moving right
        if (NearlyEqual(transform.position.x, rightPosition.x, 0.1f) && moveRight) {
            moveRight = false;

            // Wait before moving left
            StartCoroutine(DelayBeforeMove(false));
        }

        // Stop moving left
        if (NearlyEqual(transform.position.x, leftPosition.x, 0.1f) && moveLeft) {
            moveLeft = false;

            // Wait before moving right
            StartCoroutine(DelayBeforeMove(true));
        }

        Debug.Log(attack);

        if (attack) {
            // Get distance to player
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // DEBUG
            Debug.Log(distance);
            Debug.DrawRay(transform.position, player.transform.position - transform.position, (distance >= playerLoseDistance) ? Color.red : Color.green);
            
            // If player is far from the enemy
            if (distance >= playerLoseDistance) {
                attack = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;

        // Atack player if he is in attack area
        if (otherTag == "Player") {
            attack = true;
        }
    }

    private void FixedUpdate()
    {
        // Patroling or attacking
        if (attack) {
            Attack();
        }
        else {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Move to right patrol position
        if (moveRight) {
            // Get direction to right position
            Vector3 rightDirection = (rightPosition - transform.position).normalized;

            LookAtDirection(rightDirection);
            rigidBody2D.MovePosition(transform.position + rightDirection * speed * Time.fixedDeltaTime);
            
            // DEBUG
            Debug.DrawRay(transform.position, rightPosition - transform.position);
        }

        // Move to left patrol position
        if (moveLeft) {
            // Get direction to left position
            Vector3 leftDirection = (leftPosition - transform.position).normalized;

            LookAtDirection(leftDirection);
            rigidBody2D.MovePosition(transform.position + leftDirection * speed * Time.fixedDeltaTime);

            // DEBUG
            Debug.DrawRay(transform.position, leftPosition - transform.position);
        }
    }

    private void Attack()
    {
        // Find the direction to the place above player
        Vector3 direction = ((player.transform.position + Vector3.up * distanceFromPlayer) - transform.position).normalized;

        // Look at the player
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        LookAtDirection(lookDirection);

        // Stay above player
        rigidBody2D.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }

    private void LookAtDirection(Vector3 direction)
    {
        // Set enemy rotation to face the player
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    private bool NearlyEqual(float a, float b, float delta)
    {
        return Mathf.Abs(a - b) < delta;
    }

    private IEnumerator DelayBeforeMove(bool goRight)
    {
        yield return new WaitForSeconds(delayBeforeMove);

        // Change direction of movement
        if (goRight) {
            moveRight = true;
        }
        else {
            moveLeft = true;
        }
    }
}
