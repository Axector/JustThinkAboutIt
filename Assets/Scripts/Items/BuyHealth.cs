using UnityEngine;

public class BuyHealth : DefaultClass
{
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private MeshRenderer[] tooltipText;
    [SerializeField]
    private GameObject healthDrop;
    [SerializeField]
    private GameObject healthDropPosition;
    [SerializeField]
    private int[] price;

    private Player player;
    private BoxCollider2D boxCollider;
    private bool bFirstPurchase = true;
    private int selectedTooltip = 0;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        boxCollider = FindObjectOfType<BoxCollider2D>();

        // Set order in layer to see text above tooltip
        foreach (MeshRenderer text in tooltipText) {
            text.sortingOrder = 7;
            text.gameObject.SetActive(false);
        }

        // Set first text visible
        tooltipText[0].gameObject.SetActive(true);
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
        if (tooltip.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            BuyHealthPoints();
        }
    }

    private void BuyHealthPoints()
    {
        // Get free health points for the first time
        if (bFirstPurchase) {
            bFirstPurchase = false;

            // Drop health item
            Instantiate(
                healthDrop,
                healthDropPosition.transform.position,
                Quaternion.identity
            );

            // Switch to next tooltip text
            tooltipText[selectedTooltip].gameObject.SetActive(false);
            selectedTooltip++;
            tooltipText[selectedTooltip].gameObject.SetActive(true);
        }
        // Buy health if player has enough money
        else if (!bFirstPurchase && player.AddMoney(-price[selectedTooltip])) {
            // Drop health item
            Instantiate(
                healthDrop,
                healthDropPosition.transform.position,
                Quaternion.identity
            );

            // Amount of bought products is limited
            if (selectedTooltip + 1 >= tooltipText.Length) {
                boxCollider.enabled = false;
                tooltip.SetActive(false);
            }
        }
    }
}
