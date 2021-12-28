using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moving : Fighter
{

    protected BoxCollider2D boxCollider;
    protected SpriteRenderer sprite;
    protected RaycastHit2D hit;
    protected Vector3 deltaMove;

    [SerializeField]
    protected float ySpeed = 0.75f;
    [SerializeField]
    protected float xSpeed = 1f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void UpdateMovement(Vector3 input)
    {
        deltaMove = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        FlipSprite();

        // Apply push force
        deltaMove += forceDirection;

        // Reduce push force to stop it
        forceDirection = Vector3.Lerp(forceDirection, Vector3.zero, backForceSpeed);

        // Check if player is colliding creature or wall (y axis)
        hit = Physics2D.BoxCast(
            transform.position,
            boxCollider.size,
            0,
            new Vector2(0, deltaMove.y),
            Mathf.Abs(deltaMove.y * Time.fixedDeltaTime),
            LayerMask.GetMask("Blocker")
        );

        // If player is not colliding anything (y axis)
        if (hit.collider == null) {
            // Vertical movement
            transform.Translate(0, deltaMove.y * Time.deltaTime, 0);
        }

        // Check if player is colliding creature or wall (x axis)
        hit = Physics2D.BoxCast(
            transform.position,
            boxCollider.size,
            0,
            new Vector2(deltaMove.x, 0),
            Mathf.Abs(deltaMove.x * Time.fixedDeltaTime),
            LayerMask.GetMask("Blocker")
        );

        // If player is not colliding anything (x axis)
        if (hit.collider == null) {
            // Horizontal movement
            transform.Translate(deltaMove.x * Time.deltaTime, 0, 0);
        }
    }

    protected virtual void FlipSprite()
    {
        // Change look direction if creature is moving to the right or left
        if (deltaMove.x > 0) {
            sprite.flipX = true;
        }
        else if (deltaMove.x < 0) {
            sprite.flipX = false;
        }
    }
}
