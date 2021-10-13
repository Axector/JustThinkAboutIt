using System.Collections;
using UnityEngine;

public class SimpleEnemy : AEnemy
{
    public Vector3 startPosition;

    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float slowDownSpeed = 0.1f;
    [SerializeField]
    private float agressionDistance = 1f;
    [SerializeField]
    private float distanceToGoBack = 20f;

    private bool bMoveToPosition = false;

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Go to the player if it is alive
        if (player.IsAlive && canAttack) {
            MoveToPlayer();
        }
        else if (!player.IsAlive) {
            SetVelocity(Vector2.zero);
        }

        // move to start position after losing player
        if (bMoveToPosition) {
            MoveToPosition();
        }
    }

    protected override void OnCollisionStay2D(Collision2D other)
    {
        string otherTag = other.gameObject.tag;

        // Checks if an enemy can attack then attacks
        if (otherTag == "Player" && canAttack && player.IsAlive) {
            DoDamage();
            canAttack = false;
            StartCoroutine(DelayBeforeAttack());

            // Slow down after attack
            rigidBody2D.velocity /= 2;
            rigidBody2D.velocity *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string otherTag = other.gameObject.tag;
        Debug.Log(otherTag);
        rigidBody2D.AddForce(
            (other.gameObject.transform.position - transform.position).normalized * -50, 
            ForceMode2D.Impulse
        );
    }

    private void MoveToPlayer()
    {
        // Get distance from enemy to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        float distanceToStart = Vector2.Distance(player.transform.position, startPosition);

        // DEBUG
        Debug.DrawRay(transform.position, player.transform.position - transform.position, (distanceToPlayer <= agressionDistance) ? Color.red : Color.green);
        Debug.DrawRay(player.transform.position, startPosition - player.transform.position);

        // Move to player when it is near the enemy
        if (distanceToPlayer <= agressionDistance && distanceToStart <= distanceToGoBack) {
            // Stop moving to start position
            bMoveToPosition = false;
            enemyCollider2D.isTrigger = false;

            // Get direction to the player
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Look at player direction
            LookAtDirection(direction);

            SetVelocity(direction * speed * Time.fixedDeltaTime, true);

            setMaxSpeed();
        }
        else if (distanceToStart <= distanceToGoBack) {
            LowerSpeedX();
            LowerSpeedY();
        }
        else {
            // Move to start position
            bMoveToPosition = true;
            enemyCollider2D.isTrigger = true;
        }
    }

    private void LookAtDirection(Vector2 direction)
    {
        // Set enemy rotation to face the player
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    private void setMaxSpeed()
    {
        Vector2 newVelocity = rigidBody2D.velocity;

        // If horizontal movement speed is maximal
        if (Mathf.Abs(newVelocity.x) >= maxSpeed) {
            newVelocity = new Vector2((newVelocity.x > 0 ? maxSpeed : -maxSpeed), newVelocity.y);
        }

        // If vertical movement speed is maximal
        if (Mathf.Abs(newVelocity.y) >= maxSpeed) {
            newVelocity = new Vector2(newVelocity.x, (newVelocity.y > 0 ? maxSpeed : -maxSpeed));
        }

        SetVelocity(newVelocity);
    }

    private void LowerSpeedX() 
    {
        float velocityX = rigidBody2D.velocity.x;

        // Lower horizontal movement speed
        if (velocityX > 0) {
            SetVelocity(new Vector2(-slowDownSpeed, 0), true);
        }
        else if (velocityX < 0) {
            SetVelocity(new Vector2(slowDownSpeed, 0), true);
        }

        // If horizontal movement speed is nearly 0, set it to 0
        if (
            velocityX != 0 &&
            velocityX > -0.5 &&
            velocityX < 0.5
        ) {
            SetVelocity(new Vector2(0, rigidBody2D.velocity.y));
        }
    }

    private void LowerSpeedY()
    {
        float velocityY = rigidBody2D.velocity.y;

        // Lower vertical movement speed
        if (velocityY > 0) {
            SetVelocity(new Vector2(0, -slowDownSpeed), true);
        }
        else if (velocityY < 0) {
            SetVelocity(new Vector2(0, slowDownSpeed), true);
        }

        // If vertical movement speed is nearly 0, set it to 0
        if (
            velocityY != 0 &&
            velocityY > -0.5 &&
            velocityY < 0.5
        ) {
            SetVelocity(new Vector2(rigidBody2D.velocity.x, 0));
        }
    }

    private void MoveToPosition()
    {
        // Get direction to the starting position
        Vector3 direction = (startPosition - transform.position).normalized;

        // Look at starting position direction
        LookAtDirection(direction);

        // Move to starting position
        rigidBody2D.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }
}
