using UnityEngine;

public class KeyObjectDestroy : DefaultClass
{
    [SerializeField]
    private PolygonCollider2D polygonCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite brokenSprite;
    [SerializeField]
    private Color brokenColor;
    [SerializeField]
    protected GameObject[] drop;
    [SerializeField]
    protected float dropPercent;
    [SerializeField]
    protected int maxDropAmount;

    private bool isDestroyed = false;

    public bool IsDestroyed { get => isDestroyed; }

    private void Update()
    {
        if (isDestroyed && polygonCollider.enabled) {
            polygonCollider.enabled = false;
            spriteRenderer.sprite = brokenSprite;
            spriteRenderer.color = brokenColor;

            Drop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack") {
            isDestroyed = true;
        }
    }

    private void Drop()
    {
        int dropCount = drop.Length;

        if (Chance(dropPercent) && dropCount != 0) {
            int randomAmount = Random.Range(1, maxDropAmount);

            // Create random amount of drop items
            for (int i = 0; i < randomAmount; i++) {
                // Get random drop item from list with drop
                GameObject randomDrop = drop[Random.Range(0, dropCount)];

                Instantiate(
                    randomDrop,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
    }
}
