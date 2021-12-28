using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : CollidableObject
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackForce;
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
        enemyAnimator.Play("Attack");
    }
}
