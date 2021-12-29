using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : CollidableObject
{
    protected bool collected = false;

    protected virtual void OnCollect()
    {
        if (!collected) {
            collected = true;
        }
    }
}
