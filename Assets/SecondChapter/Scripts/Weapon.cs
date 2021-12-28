using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CollidableObject
{
    [SerializeField]
    private int[] weaponDamage;
    [SerializeField]
    private float[] weaponForce;
    [SerializeField]
    private float[] attackCooldown;
    [SerializeField]
    private Sprite[] sprites;

    // DEBUG (No need in serialize)
    [SerializeField]
    private int level;

    private SpriteRenderer sprite;
    private Animator animator;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();

        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        level = PlayerPrefs.GetInt("weapon_level", level);
        sprite.sprite = sprites[level];
        animator.SetFloat("attackSpeed", 1 / attackCooldown[level]);
    }

    protected override void Update()
    {
        base.Update();

        // Next attack can be done only after some time
        if (Input.GetKeyDown(KeyCode.Space)) { 
            if (Time.time - lastSwing > attackCooldown[level]) {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollision(Collider2D other)
    {
        if (other.gameObject.tag == "Fighter") { 
            if (other.name == "Player") {
                return;
            }

            // Create object to deal damage
            DoDamage doDamage = new DoDamage {
                damage = weaponDamage[level],
                position = transform.position,
                attackForce = weaponForce[level]
            };

            other.SendMessage("GetDamage", doDamage);
        }
    }

    private void Swing()
    {
        animator.Play("Swing");
    }
}
