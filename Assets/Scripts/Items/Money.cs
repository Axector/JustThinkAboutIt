using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField]
    private int amount = 1000;

    private Player player;
    private SpriteRenderer spriteRenderer;
    private Popup textPopup;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        textPopup = GetComponent<Popup>();
    }

    public void earn()
    {
        player.setMoney(amount);

        // Show text popup
        textPopup.showPopup(amount.ToString());
    }
}
