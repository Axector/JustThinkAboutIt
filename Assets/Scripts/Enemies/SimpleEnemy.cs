using System.Collections;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 40;
    [SerializeField]
    private float attackDelay = 1f;

    private Player player;
    private bool canAttack = true;

    private void Awake()
    {
        // Get the player when the enemy has spawned
        player = FindObjectOfType<Player>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (canAttack) {
            doDamage();
            canAttack = false;
            StartCoroutine(delayBeforeAttack());
        }
    }

    private IEnumerator delayBeforeAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void doDamage()
    {
        // Deal damage to the player
        player.setHealth(-damage);
    }
}
