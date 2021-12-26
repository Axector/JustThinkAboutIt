using UnityEngine;

public class BuyHealth : DefaultClass
{
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private MeshRenderer tooltipText;
    [SerializeField]
    private GameObject healthDrop;
    [SerializeField]
    private GameObject healthDropPosition;
    [SerializeField]
    private int price;

    private Player player;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        boxCollider = FindObjectOfType<BoxCollider2D>();
        tooltipText.sortingOrder = 7;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            tooltip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            tooltip.SetActive(false);
        }
    }

    private void Update()
    {
        if (tooltip.activeSelf && Input.GetKeyUp(KeyCode.E)) {
            BuyHealthPoints();
        }
    }

    private void BuyHealthPoints()
    {
        // Buy health if player has enough money
        if (player.AddMoney(-price)) {
            // Drop health item
            Instantiate(
                healthDrop,
                healthDropPosition.transform.position,
                Quaternion.identity
            );

            // Health can be bought only once
            boxCollider.enabled = false;
            tooltip.SetActive(false);
        }
    }
}
