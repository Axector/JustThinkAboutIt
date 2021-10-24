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
    private int damage = 10;
    [SerializeField]
    private float delayToRestart = 3f;
    [SerializeField]
    private GameObject fireUpParticles;
    [SerializeField]
    private Vector3 attackUpOffset;
    [SerializeField]
    private GameObject fireLeftParticles;
    [SerializeField]
    private GameObject fireRightParticles;
    [SerializeField]
    private Vector3 attackHorizontalOffset;
    [SerializeField]
    private GameObject fireBall;
    [SerializeField]
    private Vector3 attackDownOffset;

    private int health;
    private bool isAlive = true;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool isCutscene;

    public float PlayerSpeed { get => playerSpeed; }
    public float JumpForce { get => jumpForce; }
    public int MaxHealth { get => maxHealth; }
    public int Health { get => health; }
    public bool IsAlive { get => isAlive; }
    public int Damage { get => damage; }

    private void Awake()
    {
        // Get needed components
        playerController = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // Play animation if the player was attacked
        if (hp < 0 && isAlive) {
            animator.Play("Player_Hurt");
        }
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

    public void AttackUp()
    {
        InstatiateAttackParticles(
            fireUpParticles,
            (spriteRenderer.flipX)
                ? new Vector3(-attackUpOffset.x, attackUpOffset.y, 0)
                : attackUpOffset,
            true
        );
    }

    public void StandAttackHorizontal()
    {
        AttackHorizontal(true);
    }

    public void RunAttackHorizontal()
    {
        AttackHorizontal(false);
    }

    private void AttackHorizontal(bool isStanding)
    {
        // Get offset for left or right attack
        Vector3 offset = (spriteRenderer.flipX)
            ? new Vector3(-attackHorizontalOffset.x, attackHorizontalOffset.y)
            : attackHorizontalOffset;

        // Create attack particles
        InstatiateAttackParticles(
            (spriteRenderer.flipX) 
                ? fireLeftParticles 
                : fireRightParticles,
            isStanding
                ? offset
                : new Vector3(offset.x, offset.y - 0.01f),
            true
        );
    }

    public void AttackDown()
    {
        InstatiateAttackParticles(
            fireBall,
            attackDownOffset,
            false
        );
    }

    private void InstatiateAttackParticles(GameObject particles, Vector3 attackOffset, bool underPlayer)
    {
        if (underPlayer) {
            // Create particles in correct position under player game object
            Instantiate(
                particles,
                transform.position,
                Quaternion.Euler(-90, 0, 0),
                transform
            ).transform.position += attackOffset;
        }
        else {
            // Create particles in correct position
            Instantiate(
                particles,
                transform.position,
                Quaternion.Euler(-90, 0, 0)
            ).transform.position += attackOffset;
        }
    }
}
