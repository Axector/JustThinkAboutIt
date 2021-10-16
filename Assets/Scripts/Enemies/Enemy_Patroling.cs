using System.Collections;
using UnityEngine;

public class Enemy_Patroling : AEnemy
{
    [SerializeField]
    protected float delayBeforeMove = 1f;
    [SerializeField]
    protected Vector3[] patrolPositions;

    protected int currentPatrolPositionIndex = 0;
    protected bool move = true;

    protected virtual void OnEnable()
    {
        move = true;
    }

    protected virtual void Update()
    {
        // Stop moving when achieved the position
        if (
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
        if (move) {
            // Get direction to right position
            Vector3 rightDirection = (patrolPositions[currentPatrolPositionIndex] - transform.localPosition).normalized;

            LookAtDirection(rightDirection);
            rigidBody2D.MovePosition(transform.position + rightDirection * speed * Time.fixedDeltaTime);

            // DEBUG
            Debug.DrawRay(transform.position, patrolPositions[currentPatrolPositionIndex] - transform.localPosition);
        }
    }

    protected IEnumerator DelayBeforeMove()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        // Start moving to the next position after delay
        move = true;
    }
}
