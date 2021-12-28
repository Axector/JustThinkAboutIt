using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : CollidableObject
{
    protected bool collected = true;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E)) {
            OnCollect();
        }
    }

    protected virtual void OnCollect()
    {
        if (!collected) {
            collected = true;
        }
    }
}
