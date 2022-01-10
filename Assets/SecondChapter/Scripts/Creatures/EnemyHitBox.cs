using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : CollidableObject
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float attackForce;
    [SerializeField]
    private Animator enemyAnimator;

    protected override void OnCollision(Collider2D other)
    {
        if (other.name == "Player") {
            // Create object to deal damage
            DoDamage doDamage = new DoDamage
            {
                damage = damage,
                position = transform.position,
                attackForce = attackForce
            };

            other.SendMessage("GetDamage", doDamage);

            PlayAttackAnimation();
        }
    }

    protected void PlayAttackAnimation()
    {
        // Play attack animation
        enemyAnimator.Play("Attack");
    }
}
