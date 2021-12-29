using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShop : CollectableObject
{
    [SerializeField]
    private MeshRenderer[] tooltipText;
    [SerializeField]
    private int[] prices;
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private AudioClip audioClip;

    private int purchaseIndex = 0;

    private AudioSource audioSource;
    private Popup popup;

    protected override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
        popup = GetComponent<Popup>();

        // Set order in layer to see text above tooltip
        foreach (MeshRenderer text in tooltipText) {
            text.sortingLayerName = "Weapon";
            text.sortingOrder = 2;
            text.gameObject.SetActive(false);
        }

        // Set first text visible
        tooltipText[0].gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        
        // Show tooltip if player is near
        tooltip.SetActive(bCollidingPlayer && !collected);
    }

    protected override void OnCollision(Collider2D other)
    {
        if (other.name == "Player" && !collected) {
            int playerHealth = PlayerPrefs.GetInt("player_health", 0);
            int playerMaxHealth = PlayerPrefs.GetInt("player_max_health", 0);
            int playerMoney = PlayerPrefs.GetInt("player_run_money", 0) + PlayerPrefs.GetInt("player_money", 0);

            if (
                Input.GetKeyDown(KeyCode.E) && 
                playerMoney >= prices[purchaseIndex] &&
                playerHealth != playerMaxHealth
            ) {
                // Create object to give player health
                DoDamage giveHealth = new DoDamage {
                    damage = -10,
                    position = transform.position,
                    attackForce = 0
                };

                // Show popup
                popup.ShowPopup(giveHealth.damage.ToString(), transform);

                other.SendMessage("GetDamage", giveHealth);

                OnCollect();
            }
        }
    }

    protected override void OnCollect()
    {
        purchaseIndex++;

        // Play collection sound
        PlaySound(audioSource, audioClip);

        // Player can buy limited health amount
        if (purchaseIndex + 1 >= prices.Length) {
            collected = true;
            tooltip.SetActive(false);
        }
    }
}
