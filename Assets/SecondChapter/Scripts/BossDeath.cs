using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : Enemy
{
    [SerializeField]
    private GameObject[] hideOnBossDeath;
    [SerializeField]
    private PlayerHealth healthBar;

    private bool hidden = false;

    private void Update()
    {
        // Check if boss is alive and objects are not hidden yet
        if (!isAlive && !hidden) {
            hidden = true;

            HideObjects();
        }
    }

    private void HideObjects()
    {
        // Hide given objects after boss death 
        foreach (GameObject obj in hideOnBossDeath) {
            obj.SetActive(false);
        }
    }

    protected override void GetDamage(DoDamage damage)
    {
        base.GetDamage(damage);

        healthBar.CheckHealth();
    }
}
