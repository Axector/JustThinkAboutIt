using System.Collections;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 40;
    [SerializeField]
    private float attackForce = 4f;
    [SerializeField]
    private float attackDelay = 1f;
    [SerializeField]
    private float agressionDistance = 1f;

    private Player player;
    private bool canAttack = true;

    private void Awake()
    {
        // Get the player when the enemy has spawned
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        // Go to the player if it is alive
        if (player.IsAlive && canAttack) {
            moveToPlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // Checks if an enemy can attack then attack
        if (canAttack) {
            doDamage();
            canAttack = false;
            StartCoroutine(delayBeforeAttack());

            // Rotate an enemy to the player
            Vector3 direction = gameObject.transform.position - player.gameObject.transform.position;
            float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

            // Push the player after attack
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * attackForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator delayBeforeAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void moveToPlayer()
    {
        // Get distance from enemy to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Move to player when it is near the enemy
        if (distanceToPlayer <= agressionDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 1f * Time.fixedDeltaTime);
        }
    }

    private void doDamage()
    {
        // Deal damage to the player
        player.setHealth(-damage);
    }
}
