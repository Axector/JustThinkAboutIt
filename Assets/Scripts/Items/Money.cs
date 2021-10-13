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
        textPopup = GetComponent<Popup>();
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Earn()
    {
        player.AddMoney(amount);

        // Show text popup
        textPopup.ShowPopup(amount.ToString());
    }
}
