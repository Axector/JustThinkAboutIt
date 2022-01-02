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

    public virtual void OnCollect(Collider2D other)
    {
        if (!collected) {
            collected = true;

            // Play collection sound
            PlaySound(audioSource, audioClip);
        }
    }
}
