using UnityEngine;

public class GameController : MonoBehaviour
{
    public float damageIncreaseFactor = 1f;

    private void Start()
    {
        IncreaseDamage();
    }

    private void IncreaseDamage()
    {
        AEnemy[] enemies = FindObjectsOfType<AEnemy>();

        foreach (AEnemy enemy in enemies) {
            enemy.IncreaseDamage(damageIncreaseFactor);
        }
    }
}
