using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : CollectableObject
{
    [SerializeField]
    private int coinsAmount;
    [SerializeField]
    private float percentage = 50f;

    private Animator animator;
    private Popup popup;
    private bool bOpened = false;
    private bool bFull = false;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        popup = GetComponent<Popup>();
    }

    protected override void OnCollision(Collider2D other)
    {
        // Open full or empty chest and leave it opened
        if (other.name == "Player" && !collected) {
            if (!bOpened) {
                bOpened = true;

                // Chance that chest will be full
                if (Chance(percentage)) {
                    animator.Play("ChestOpen_Full");
                    bFull = true;
                }
                else {
                    animator.Play("ChestOpen_Empty");
                }
            }

            // Get coins from chest
            if (Input.GetKeyDown(KeyCode.E)) {
                OnCollect();
            }
        }
    }

    protected override void OnCollect()
    {
        base.OnCollect();

        if (bFull) {
            animator.Play("ChestOpen_Empty");

            popup.ShowPopup(coinsAmount.ToString(), transform);

            PlayerPrefs.SetInt("player_money", PlayerPrefs.GetInt("player_money", 0) + coinsAmount);
        }
    }
}
