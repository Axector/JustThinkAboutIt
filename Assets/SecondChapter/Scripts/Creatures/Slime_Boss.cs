using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Boss : DefaultClass
{
    [SerializeField]
    private GameObject[] minions;
    [SerializeField]
    private GameObject[] minions2;

    private Fighter boss;
    private bool secondPhase;
    private bool thirdPhase;

    private void Start()
    {
        boss = GetComponent<Fighter>();
    }

    private void Update()
    {
        // Start second phase
        if (!secondPhase && boss.healthPoints <= boss.maxHealthPoints * 2 / 3) {
            SecondPhase();
        }

        // Start third phase
        if (!thirdPhase && boss.healthPoints <= boss.maxHealthPoints / 3) {
            ThirdPhase();
        }

        // Show minions hide minions on boss death
        if (!boss.IsAlive) {
            foreach (GameObject minion in minions) {
                if (minion != null) {
                    minion.SetActive(false);
                }
            }

            foreach (GameObject minion in minions2) {
                if (minion != null) {
                    minion.SetActive(false);
                }
            }
        }
    }

    private void SecondPhase()
    {
        secondPhase = true;

        // Show minions
        foreach (GameObject minion in minions) { 
            minion.SetActive(true);
        }
    }

    private void ThirdPhase()
    {
        thirdPhase = true;

        // Show minions
        foreach (GameObject minion in minions2) { 
            minion.SetActive(true);
        }
    }
}
