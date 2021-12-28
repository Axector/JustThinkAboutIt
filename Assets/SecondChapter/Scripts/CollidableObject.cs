using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollidableObject : DefaultClass
{
    [SerializeField]
    private ContactFilter2D filter;

    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        // Check collision, if any object is overlapping
        boxCollider.OverlapCollider(filter, hits);

        // Check each collision hit (max = 10)
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null) {
                continue;
            }

            OnCollision(hits[i]);

            hits[i] = null;
        }
    }

    protected virtual void OnCollision(Collider2D other)
    {

    }
}
