using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField]
    private int amount = 1000;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void earn()
    {
        player.setMoney(amount);
    }
}
