using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class Player_TopDown : Moving
{
    [SerializeField]
    private PlayerHealth healthBar;
    [SerializeField]
    private GameObject fadingScreen;
    [SerializeField]
    private ContactFilter2D filter;
    [SerializeField]
    private Joystick joystick;

    private Animator animator;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();

        // Apply health power up
        if (PlayerPrefs.GetInt("health_power_up_2", 0) == 1) { 
            healthPoints *= 2;
            maxHealthPoints *= 2;
        }

        PlayerPrefs.SetInt("player_max_health", maxHealthPoints);
        healthPoints = PlayerPrefs.GetInt("player_health", maxHealthPoints);

        healthBar.CheckHealth();
    }

    private void FixedUpdate()
    {
        float xVelocity; 
        float yVelocity;

        // Horizontal movement values from joystick
        if (joystick.Horizontal >= -0.2f && joystick.Horizontal <= 0.2f) {
            xVelocity = 0;
        }
        else {
            xVelocity = joystick.Horizontal;
        }

        // Vertical movement values from joystick
        if (joystick.Vertical >= -0.2f && joystick.Vertical <= 0.2f) {
            yVelocity = 0;
        }
        else {
            yVelocity = joystick.Vertical;
        }

        UpdateMovement(new Vector3(xVelocity, yVelocity, 0));

        // Player running animation
        animator.SetInteger("velocity", (xVelocity != 0) ? 1 : ((yVelocity != -0) ? 1 : 0));
    }

    protected override void FlipSprite()
    {
        // Change look direction if player is moving to the right or left
        if (deltaMove.x > 0) {
            transform.localScale = Vector3.one;
        }
        else if (deltaMove.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void GetDamage(DoDamage damage)
    {
        base.GetDamage(damage);

        healthBar.CheckHealth();
    }

    protected override IEnumerator Death()
    {
        // Animation of player death
        animator.SetBool("isAlive", isAlive);

        // Apply second live power up
        if (PlayerPrefs.GetInt("lives_power_up_2", 0) == 1) {
            yield return new WaitForSeconds(2f);

            isAlive = true;
            healthPoints = maxHealthPoints;
            animator.SetBool("isAlive", isAlive);

            PlayerPrefs.SetInt("lives_power_up_2", 0);
            PlayerPrefs.SetInt("player_health", healthPoints);

            Destroy(GameObject.FindGameObjectWithTag("LivesPowerUp"));
        }
        else {
            // To load menu after results page
            PlayerPrefs.SetInt("next_level", 1);
            // To not earn money of current room
            PlayerPrefs.GetInt("save_money", 0);

            yield return new WaitForSeconds(1f);

            // Fading screen after death
            fadingScreen.SetActive(true);

            yield return new WaitForSeconds(3f);

            // Load results scene
            SceneManager.LoadScene(11);
        }
    }

    public void Interact()
    {
        boxCollider.OverlapCollider(filter, hits);

        // Check each collision hit (max = 10)
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null) {
                continue;
            }

            // Check if collectible is of type CollectableObject
            CollectableObject collectable = hits[i].GetComponent<CollectableObject>();
            if (collectable != null) {
                collectable.OnCollect(boxCollider);
            }

            hits[i] = null;
        }
    }
}
