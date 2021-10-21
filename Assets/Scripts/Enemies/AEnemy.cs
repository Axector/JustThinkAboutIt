using UnityEngine;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float speedIncrease;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected float delayBeforeAttack = 0.5f;

    protected Player player;
    protected Popup textPopup;
    protected Collider2D collider2d;
    protected Rigidbody2D rigidBody2D;
    protected Vector3 startingPosition;
    protected float health;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        textPopup = GetComponent<Popup>();
        collider2d = GetComponent<Collider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        health = maxHealth;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        // Checks if an enemy can attack then attacks
        if (other.gameObject.tag == "Player" && player.IsAlive) {
            DoDamage();
        }
    }

    protected bool NearlyEqual(float a, float b, float delta)
    {
        return Mathf.Abs(a - b) < delta;
    }

    protected void LookAtDirection(Vector3 direction)
    {
        // Set enemy rotation to face the player
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    public void DoDamage()
    {
        // Deal damage to the player
        player.AddHealth(-damage);

        // Show damage popup on player
        textPopup.ShowPopup(damage.ToString(), player.transform);
    }

    public virtual void SetHealth(int hp)
    {
        health += hp;

        // Enemy dies after health is < 0
        if (health <= 0) {
            Destroy(gameObject);
        }

        // Health cannot be more than maximum
        if (health > maxHealth) {
            health = maxHealth;
        }
    }
}
