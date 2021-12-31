using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : CollidableObject
{
    [SerializeField]
    protected AudioClip audioClip;

    protected bool collected = false;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnCollect()
    {
        if (!collected) {
            collected = true;

            // Play collection sound
            PlaySound(audioSource, audioClip);
        }
    }
}
