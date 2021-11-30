using System.Collections;
using UnityEngine;

public class PassedShow : DefaultClass
{
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject otherGameObject = other.gameObject;
        
        if (
            otherGameObject.tag == "Player" &&
            otherGameObject.transform.position.y > transform.position.y
        ) {
            boxCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
    }
}
