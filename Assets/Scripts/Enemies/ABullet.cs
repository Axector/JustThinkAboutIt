using UnityEngine;

public abstract class ABullet : DefaultClass
{
    [SerializeField]
    protected float speed;

    protected Rigidbody2D rigidBody2D;
    protected AEnemy parent;

    public void Setup(AEnemy parent)
    {
        this.parent = parent;
    }

    protected virtual void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        rigidBody2D.velocity = transform.right * speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Get tag of other
        string tag = other.gameObject.tag;

        // Destroy the bullet when it hits something
        if (tag == "SolidBlock") {
            Destroy(gameObject);
        }

        if (tag == "Player") {
            // Deal damage to the player
            parent.DoDamage();

            Destroy(gameObject);
        }
    }
}
