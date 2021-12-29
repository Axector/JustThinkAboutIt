using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : CollectableObject
{
    [SerializeField]
    private GameObject objectToDisable;
    [SerializeField]
    private AudioClip audioClip;

    private Animator animator;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnCollision(Collider2D other)
    {
        // Lever can be used only by player and only once
        if (other.name == "Player" && !collected) {
            // Use lever
            if (Input.GetKeyDown(KeyCode.E)) {
                OnCollect();
            }
        }
    }

    protected override void OnCollect()
    {
        base.OnCollect();

        // Play use animation
        animator.Play("UseLever");
        PlaySound(audioSource, audioClip);

        objectToDisable.SetActive(false);
    }
}
