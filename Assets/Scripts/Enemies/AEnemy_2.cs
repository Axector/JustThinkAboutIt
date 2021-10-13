using System.Collections;
using UnityEngine;

public class AEnemy_2 : MonoBehaviour
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int health;
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
    protected Rigidbody2D rigidBody2D;
    protected bool canAttack = true;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        textPopup = GetComponent<Popup>();
        rigidBody2D = GetComponent<Rigidbody2D>();
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
        yield return new WaitForSeconds(delayBeforeAttack);
        canAttack = true;
    }

    protected void DoDamage()
    {
        // Deal damage to the player
        player.AddHealth(-damage);

        // Show damage popup on player
        textPopup.ShowPopup(damage.ToString(), player.transform);
    }
}
