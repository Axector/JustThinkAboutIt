using System.Collections;
using UnityEngine;

public class NPC_Dog : NPC
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private Rigidbody2D rigidBody;

    private void OnEnable()
    {
        animator.Play("EnterSceneWithFairy");

        StartCoroutine(EnableCollider());
    }

    public IEnumerator EnableCollider()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        boxCollider.enabled = true;
        rigidBody.simulated = true;
    }
}
