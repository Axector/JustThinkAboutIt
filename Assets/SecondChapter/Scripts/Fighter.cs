using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : DefaultClass
{
    public int healthPoints = 10;
    public int maxHealthPoints = 10;
    public float backForceSpeed = 0.2f;

    [SerializeField]
    protected float immuneTime = 1f;

    protected float lastImmune;
    protected Vector3 forceDirection;
    protected Popup popup;
    protected bool isAlive = true;

    protected virtual void Awake()
    {
        popup = GetComponent<Popup>();
    }

    protected virtual void GetDamage(DoDamage damage)
    {
        // Creature will receive next damage only after some time
        if (Time.time - lastImmune > immuneTime && isAlive) {
            lastImmune = Time.time;
            healthPoints -= damage.damage;
            forceDirection = (transform.position - damage.position).normalized * damage.attackForce;

            if (damage.damage != 0) {
                popup.ShowPopup(damage.damage.ToString(), transform);
            }

            // Die if health is < 0
            if (healthPoints <= 0) {
                healthPoints = 0;
                isAlive = false;
                StartCoroutine(Death());
            }

            if (gameObject.name == "Player") {
                PlayerPrefs.SetInt("player_health", healthPoints);
            }
        }
    }
    
    protected virtual IEnumerator Death()
    {
        yield return new WaitForSeconds(0f);
    }
}
