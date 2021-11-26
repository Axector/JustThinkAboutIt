using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
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
    private float delayBeforeStartSound;
    [SerializeField]
    private AudioClip startSound;
    [SerializeField]
    private QuickMenu quickMenu;
    [SerializeField]
    private float increaseFactor = 1f;
    [SerializeField]
    private Locale[] languages;

    public float IncreaseFactor { get => increaseFactor; set => increaseFactor = value; }

    private void Start()
    {
        if (bPlayStartSound) {
            StartCoroutine(PlayStartSound());
        }
    }

    private void Awake()
    {
        // Set game language
        LocalizationSettings.SelectedLocale = languages[PlayerPrefs.GetInt("selected_lang", 0)];
        ResetPlayerStats();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            // Resume game and close menu
            if (quickMenu.Menu.activeSelf) {
                quickMenu.Resume();
            }
            // Close just controls layout
            else if (quickMenu.SettingsMenu.BOpenControls) {
                quickMenu.SettingsMenu.CloseControls();
            }
            // Close just settings menu
            else if (quickMenu.SettingsMenu.gameObject.activeSelf) {
                quickMenu.CloseSettings();
            }
            // Pause game and open menu
            else {
                quickMenu.OpenMenu();
            }
        }
    }

    private void ResetPlayerStats()
    {
        if (bFirstLevel) {
            PlayerPrefs.SetInt("player_health", player.MaxHealth);
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
