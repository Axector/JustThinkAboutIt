using System.Collections;
using UnityEngine;

public class Enemy : Moving
{
    [SerializeField]
    private float followStartDistance = 1;
    [SerializeField]
    private float maxFollowDistance = 1.5f;
    [SerializeField]
    private ContactFilter2D filter;

    private bool bFollowing;
    private bool bCollidingWithPlayer;
    private Transform player;
    private Vector3 startPosition;
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10];
    private Animator animator;

    protected override void Start()
    {
        base.Start();

        player = FindObjectOfType<Player_TopDown>().transform;
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        // No follow if dead
        if (!isAlive) {
            // Small down enemy on death
            if (transform.localScale.y > 0) {
                transform.localScale -= Vector3.one * 3f * Time.deltaTime;
            }
            else {
                transform.localScale = Vector3.zero;
            }

            // Disable all enemy animations
            hitBox.enabled = false;
            animator.SetBool("following", false);
            return;
        }

        // If an enemy see player
        if (
            Vector3.Distance(transform.position, startPosition) < maxFollowDistance &&
            Vector3.Distance(player.position, transform.position) < maxFollowDistance
        ) { 
            // if an enemy is close enough to the player
            if (Vector3.Distance(player.position, startPosition) < followStartDistance) {
                bFollowing = true;
            }

            if (bFollowing) { 
                if (!bCollidingWithPlayer) {
                    UpdateMovement((player.position - transform.position).normalized);
                }
            }
            else {
                UpdateMovement(startPosition - transform.position);
            }
        }
        else {
            UpdateMovement(startPosition - transform.position);
            bFollowing = false;
        }

        animator.SetBool("following", 
            !(NearlyEqual(deltaMove.x, 0, 0.1f) && NearlyEqual(deltaMove.y, 0, 0.1f))
        );

        bCollidingWithPlayer = false;

        CheckCollision();
    }

    private void CheckCollision()
    {
        // Check collision, if any object is overlapping
        boxCollider.OverlapCollider(filter, hits);

        // Check each collision hit (max = 10)
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null) {
                continue;
            }

            // Check if it is colliding with player
            if (hits[i].tag == "Fighter" && hits[i].name == "Player") {
                bCollidingWithPlayer = true;
            }

            hits[i] = null;
        }
    }

    protected override IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
