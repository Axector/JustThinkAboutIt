using System.Collections;
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
    private bool bSaveMoney;
    [SerializeField]
    private float delayBeforeStartSound;
    [SerializeField]
    private AudioClip startSound;
    [SerializeField]
    private QuickMenu quickMenu;
    [SerializeField]
    private float increaseFactor = 1f;

    public float IncreaseFactor { get => increaseFactor; set => increaseFactor = value; }

    private void Start()
    {
        if (bPlayStartSound) {
            StartCoroutine(PlayStartSound());
        }

        PlayerPrefs.SetInt("save_money", (bSaveMoney) ? 1 : 0);
    }

    private void Awake()
    {
        ResetPlayerStats();
    }

    private void ResetPlayerStats()
    {
        if (bFirstLevel) {
            int maxHealth = PlayerPrefs.GetInt("player_max_health", player.MaxHealth);

            PlayerPrefs.SetInt("player_health", maxHealth);
            player.AddHealth(player.MaxHealth);

            PlayerPrefs.SetInt("player_money", 0);
            player.ResetMoney();
        }
    }

    private IEnumerator PlayStartSound()
    {
        yield return new WaitForSeconds(delayBeforeStartSound);

        PlaySound(player.AudioSource, startSound);
    }
}
