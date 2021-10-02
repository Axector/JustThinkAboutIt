using System.Collections;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField]
    protected int damage = 40;
    [SerializeField]
    protected float attackDelay = 1f;

    protected Player player;
    protected bool canAttack = true;

    protected Popup textPopup;

    protected virtual void Awake()
    {
        // Get the player when the enemy has spawned
        player = FindObjectOfType<Player>();
        textPopup = GetComponent<Popup>();
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        // Checks if an enemy can attack then attacks
        if (other.gameObject.tag == "Player" && canAttack)
        {
            doDamage();
            canAttack = false;
            StartCoroutine(delayBeforeAttack());
        }
    }

    protected IEnumerator delayBeforeAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    protected void doDamage()
    {
        // Deal damage to the player
        player.setHealth(-damage);

        // Show text popup
        textPopup.showPopup(damage.ToString());
    }
}
