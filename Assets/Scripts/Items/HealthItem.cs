using UnityEngine;

public class HealthItem : DefaultClass
{
    [SerializeField]
    private int health;

    private Popup textPopup;

    private void Awake()
    {
        textPopup = GetComponent<Popup>();
    }

    public void IncreasePlayerHealth(Player player)
    {
        // Add health point to player current health
        player.AddHealth(health);

        // Show text popup
        textPopup.ShowPopup(health.ToString());
    }
}
