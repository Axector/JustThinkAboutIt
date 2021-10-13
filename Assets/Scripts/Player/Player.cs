using UnityEngine.SceneManagement;
using System.Collections;
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
    [SerializeField]
    private float delayToRestart = 3f;

    private int health;
    private bool isAlive = true;
    private PlayerController playerController;
    private Animator animator;

    public float PlayerSpeed { get => playerSpeed; }
    public float JumpForce { get => jumpForce; }
    public int MaxHealth { get => maxHealth; }
    public int Health { get => health; }
    public bool IsAlive { get => isAlive; }

    private void Awake()
    {
        // Get needed components
        playerController = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();

        // Set basic stats for player
        AddHealth(maxHealth);
        animator.SetBool("isAlive", isAlive);
    }

    private void Update()
    {
        // If player is dead animation should be started and after some seconds game restarts
        if (!isAlive) {
            animator.SetBool("isAlive", isAlive);
            StartCoroutine(RestartGame());
        }

        // If player fell down the platform
        if (transform.position.y < -4f) {
            isAlive = false;
            health = 0;
            playerController.ShowHealth();
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(delayToRestart);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddHealth(int hp)
    {
        health += hp;

        // If player is dead
        if (health <= 0) {
            health = 0;
            isAlive = false;
        }
        // Player cannot have more then max health
        else if (health > maxHealth) {
            health = maxHealth;
        }

        playerController.ShowHealth(this);
    }

    public bool AddMoney(int amount)
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
