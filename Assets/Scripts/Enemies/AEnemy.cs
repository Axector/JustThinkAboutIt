using System.Collections;
using UnityEngine;

public abstract class AEnemy : DefaultClass
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
    protected GameObject[] drop;
    [SerializeField]
    protected float dropPercent;
    [SerializeField]
    protected int maxDropAmount;
    [SerializeField]
    protected float delayBeforeAttack = 0.5f;
    [SerializeField]
    private AudioClip hurtAudio;

    protected Player player;
    protected GameController gameController;
    protected Popup textPopup;
    protected Collider2D collider2d;
    protected Rigidbody2D rigidBody2D;
    protected Animator animator;
    protected AudioSource audioSource;
    protected Vector3 startingPosition;
    protected float health;
    protected bool isAlive;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        gameController = FindObjectOfType<GameController>();
        textPopup = GetComponent<Popup>();
        collider2d = GetComponent<Collider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = spriteRenderer.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Increase stats depending on level and set some basic values
        float factor = gameController.IncreaseFactor;
        damage = (int)(damage * factor);
        maxHealth = (int)(maxHealth * factor);
        health = maxHealth;
        startingPosition = transform.position;
        isAlive = true;
        animator.SetBool("isAlive", isAlive);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        // Checks if an enemy can attack then attacks
        if (other.gameObject.tag == "Player" && player.IsAlive) {
            DoDamage();
        }
    }

    protected virtual void DestroyEnemy()
    {
        Drop();

        Destroy(gameObject);
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

        // Enemy takes damage
        if (hp < 0) {
            // Play hurt sound
            PlaySound(audioSource, hurtAudio);
        }

        // Enemy dies after health is less than 0
        if (health <= 0) {
            isAlive = false;
            animator.SetBool("isAlive", isAlive);
        }

        // Health cannot be more than maximum
        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    protected void Drop()
    {
        int dropCount = drop.Length;

        if (Chance(dropPercent) && dropCount != 0) {
            int randomAmount = Random.Range(1, maxDropAmount);

            // Create random amount of drop items
            for (int i = 0; i < randomAmount; i++) {
                // Get random drop item from list with drop
                GameObject randomDrop = drop[Random.Range(0, dropCount)];

                Instantiate(
                    randomDrop,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
    }
}
