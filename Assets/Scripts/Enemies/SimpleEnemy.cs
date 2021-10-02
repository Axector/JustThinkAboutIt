using UnityEngine;

public class SimpleEnemy : AEnemy
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float slowDownSpeed = 0.1f;
    [SerializeField]
    private float attackForce = 4f;
    [SerializeField]
    private float agressionDistance = 1f;

    private Rigidbody2D rigidBody2D;

    protected override void Awake()
    {
        base.Awake();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Go to the player if it is alive
        if (player.IsAlive && canAttack) {
            moveToPlayer();
        }
    }

    protected override void OnCollisionStay2D(Collision2D other)
    {
        // Checks if an enemy can attack then attacks
        if (other.gameObject.tag == "Player" && canAttack)
        {
            doDamage();
            canAttack = false;
            StartCoroutine(delayBeforeAttack());

            // Rotate an enemy to the player
            Vector3 direction = gameObject.transform.position - player.gameObject.transform.position;
            float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

            // Push the player after attack
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * attackForce, ForceMode2D.Impulse);

            // Slow down after attack
            rigidBody2D.velocity /= 2;
        }
    }

    private void moveToPlayer()
    {
        // Get distance from enemy to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Move to player when it is near the enemy
        if (distanceToPlayer <= agressionDistance && player.IsAlive) {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rigidBody2D.velocity += direction * speed * Time.fixedDeltaTime;

            setMaxSpeed();
        }
        else {
            lowerSpeedX();
            lowerSpeedY();
        }
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

        setVelocity(newVelocity);
    }

    private void lowerSpeedX() 
    {
        // Lower horizontal movement speed
        if (rigidBody2D.velocity.x > 0) {
            setVelocity(new Vector2(-slowDownSpeed, 0), true);
        }
        else if (rigidBody2D.velocity.x < 0) {
            setVelocity(new Vector2(slowDownSpeed, 0), true);
        }

        // If horizontal movement speed is nearly 0, set it to 0
        if (
            rigidBody2D.velocity.x != 0 &&
            rigidBody2D.velocity.x > -0.5 &&
            rigidBody2D.velocity.x < 0.5
        ) {
            setVelocity(new Vector2(0, rigidBody2D.velocity.y));
        }
    }

    private void lowerSpeedY() 
    {
        // Lower vertical movement speed
        if (rigidBody2D.velocity.y > 0) {
            setVelocity(new Vector2(0, -slowDownSpeed), true);
        }
        else if (rigidBody2D.velocity.y < 0) {
            setVelocity(new Vector2(0, slowDownSpeed), true);
        }

        // If vertical movement speed is nearly 0, set it to 0
        if (
            rigidBody2D.velocity.y != 0 &&
            rigidBody2D.velocity.y > -0.5 &&
            rigidBody2D.velocity.y < 0.5
        ) {
            setVelocity(new Vector2(rigidBody2D.velocity.x, 0));
        }
    }

    private void setVelocity(Vector2 value, bool toIncrease = false)
    {
        // If is needed to increase or to set the velocity
        if (toIncrease) {
            rigidBody2D.velocity += value;
        }
        else {
            rigidBody2D.velocity = value;
        }
    }
}
