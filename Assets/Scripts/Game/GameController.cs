using UnityEngine;

public class GameController : DefaultClass
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private bool bFirstLevel;
    [SerializeField]
    private bool bPlayStartSound;
    [SerializeField]
    private AudioClip startSound;

    public float increaseFactor = 1f;

    private void Start()
    {
        PlayStartSound();
    }

    private void Awake()
    {
        IncreaseDamage();
        ResetPlayerHealth();
    }

    private void IncreaseDamage()
    {
        AEnemy[] enemies = FindObjectsOfType<AEnemy>();

        foreach (AEnemy enemy in enemies) {
            enemy.IncreaseStats(increaseFactor);
        }
    }

    private void PlayStartSound()
    {
        if (bPlayStartSound) {
            PlaySound(player.AudioSource, startSound);
        }
    }

    private void ResetPlayerHealth()
    {
        if (bFirstLevel) {
            PlayerPrefs.SetInt("player_health", player.MaxHealth);
            player.AddHealth(player.MaxHealth);
        }
    }
}
