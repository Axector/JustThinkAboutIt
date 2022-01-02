using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : CollectableObject
{
    [SerializeField]
    private GameObject objectToDisable;

    private Animator animator;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
    }

    public override void OnCollect(Collider2D other)
    {
        if (!collected) { 
            base.OnCollect(other);

            // Play use animation
            animator.Play("UseLever");

            objectToDisable.SetActive(false);            
        }
    }
}
