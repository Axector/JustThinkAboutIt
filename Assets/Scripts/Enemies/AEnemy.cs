using System.Collections;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField]
    protected int damage = 40;
    [SerializeField]
    protected float attackDelay = 1f;

    protected Player player;
    protected Popup textPopup;
    protected Rigidbody2D rigidBody2D;
    protected Collider2D enemyCollider2D;
    protected bool canAttack = true;

    protected virtual void Awake()
    {
        // Get the player when the enemy has spawned
        player = FindObjectOfType<Player>();
        textPopup = GetComponent<Popup>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        enemyCollider2D = GetComponent<Collider2D>();
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        // Checks if an enemy can attack then attacks
        if (other.gameObject.tag == "Player" && canAttack && player.IsAlive) {
            DoDamage();
            canAttack = false;
            StartCoroutine(DelayBeforeAttack());
        }
    }

    protected IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    protected void SetVelocity(Vector2 value, bool toIncrease = false)
    {
        // If is needed to increase or to set the velocity
        if (toIncrease) {
            rigidBody2D.velocity += value;
        }
        else {
            rigidBody2D.velocity = value;
        }
    }

    protected void DoDamage()
    {
        // Deal damage to the player
        player.AddHealth(-damage);

        // Show damage popup on player
        textPopup.ShowPopup(damage.ToString(), player.transform);
    }
}
