using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class Player : DefaultClass
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
    private PlayerController playerController;
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

    public AudioClip playerAttackSound;
    public AudioClip playerAttackExplosionSound;
    public AudioClip playerHitSound;

    private int health;
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private bool isCutscene;

    public float PlayerSpeed { get => playerSpeed; }
    public float JumpForce { get => jumpForce; }
    public int MaxHealth { get => maxHealth; }
    public int Health { get => health; }
    public bool IsAlive { get => isAlive; }
    public int Damage { get => damage; }
    public bool IsCutscene { get => isCutscene; set => isCutscene = value; }
    public AudioSource AudioSource { get => audioSource;}

    private void Awake()
    {
        // Get needed components
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // Set max health if it was increased
        if (PlayerPrefs.GetInt("player_max_health", 0) == 0) {
            PlayerPrefs.SetInt("player_max_health", maxHealth);
        }
        else {
            maxHealth = PlayerPrefs.GetInt("player_max_health", maxHealth);
        }
        // Set health for current run
        AddHealth(PlayerPrefs.GetInt("player_health", maxHealth));
        // Set money for current run
        money = PlayerPrefs.GetInt("player_money", 0);

        animator.SetBool("isAlive", isAlive);
        audioSource.volume = PlayerPrefs.GetFloat("sound_volume", 1f);
    }

    private void Update()
    {
        // If player is dead animation should be started and after some seconds game restarts
        if (!isAlive) {
            animator.SetBool("isAlive", isAlive);
            PlayerPrefs.SetInt("player_health", 0);
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(delayToRestart);

        PlayerPrefs.SetInt("next_level", 0);
        SceneManager.LoadScene(10);
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

        PlayerPrefs.SetInt("player_health", health);

        playerController.ShowHealth(this);

        // Play animation if the player was attacked
        if (hp < 0 && isAlive) {
            // Play attack sound
            PlaySound(audioSource, playerHitSound);

            animator.Play("Player_Hurt");
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;

        PlayerPrefs.SetInt("player_money", money);
    }

    public void ResetMoney()
    {
        money = 0;
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

    public void AttackDown()
    {
        // Create falling fireball
        InstatiateAttackParticles(
            fireBall,
            attackDownOffset,
            false
        );
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

    private void InstatiateAttackParticles(GameObject particles, Vector3 attackOffset, bool underPlayer)
    {
        // Play attack sound
        PlaySound(audioSource, playerAttackSound);

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
