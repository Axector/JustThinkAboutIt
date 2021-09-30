using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 10f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int money = 0;

    private int health;
    private bool isAlive = true;

    public float PlayerSpeed { get => playerSpeed; }
    public float JumpForce { get => jumpForce; }
    public int Health { get => health; }
    public bool IsAlive { get => isAlive; }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        Debug.Log(health);
    }

    public void setHealth(int hp)
    {
        health += hp;

        // If player is dead
        if (health < 0) {
            isAlive = false;
        }
        // Player cannot have more then max health
        else if (health > maxHealth) {
            health = maxHealth;
        }
    }

    public bool setMoney(int amount)
    {
        money += amount;

        // If there are not enough money to pay for something
        if (money < 0) {
            money -= amount;

            return false;
        }

        return true;
    }
}
