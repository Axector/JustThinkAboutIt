using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossHitBox : EnemyHitBox
{
    [SerializeField]
    private SpriteRenderer bossSprite;
    [SerializeField]
    private Transform weaponPosition;

    protected override void OnCollision(Collider2D other)
    {
        if (other.name == "Player")
        {
            PlayAttackAnimation();

            // Deal damage in correct moment
            StartCoroutine(DealDamage(other));
        }
    }

    private void FixedUpdate()
    {
        // Change weapon position depending on boss look direction
        weaponPosition.localScale = new Vector3(
            (bossSprite.flipX) ? 1 : -1, 
            1, 
            1
        );
    }

    private IEnumerator DealDamage(Collider2D other)
    {
        yield return new WaitForSeconds(0.2f);

        
        // Create object to deal damage
        DoDamage doDamage = new DoDamage {
            damage = damage,
            position = transform.position,
            attackForce = attackForce
        };

        other.SendMessage("GetDamage", doDamage);
    }
}
