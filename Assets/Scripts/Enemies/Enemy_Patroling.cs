using System.Collections;
using UnityEngine;

public class Enemy_Patroling : AEnemy
{
    [SerializeField]
    protected float delayBeforeMove = 1f;
    [SerializeField]
    protected Vector3[] patrolPositions;
    [SerializeField]
    protected float distanceToBecomeTrigger;

    protected int currentPatrolPositionIndex = 0;
    protected bool move = true;

    protected virtual void OnEnable()
    {
        move = true;
    }

    protected virtual void Update()
    {
        // Get distance to a player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // The enemy can go through the objects, when is far away from the player
        collider2d.isTrigger = (distance >= distanceToBecomeTrigger);

        // Stop moving when achieved the position
        if (
            patrolPositions.Length > 0 &&
            NearlyEqual(transform.localPosition.x, patrolPositions[currentPatrolPositionIndex].x, 0.1f) &&
            NearlyEqual(transform.localPosition.y, patrolPositions[currentPatrolPositionIndex].y, 0.1f) &&
            move 
        ) {
            move = false;

            // Select next patrol position
            currentPatrolPositionIndex++;

            // Select first position if the last position was achieved
            if (currentPatrolPositionIndex >= patrolPositions.Length) {
                currentPatrolPositionIndex = 0;
            }

            // Wait before moving to the next position
            StartCoroutine(DelayBeforeMove());
        }
    }

    protected virtual void FixedUpdate()
    {
        // Patroling
        Patrol();
    }

    protected void Patrol()
    {
        // Move to next patrol position
        if (move && patrolPositions.Length > 0) {
            // Get direction to right position
            Vector3 rightDirection = (patrolPositions[currentPatrolPositionIndex] - transform.localPosition).normalized;

            LookAtDirection(rightDirection);
            rigidBody2D.MovePosition(transform.position + rightDirection * speed * Time.fixedDeltaTime);
        }
    }

    protected IEnumerator DelayBeforeMove()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        // Start moving to the next position after delay
        move = true;
    }
}
