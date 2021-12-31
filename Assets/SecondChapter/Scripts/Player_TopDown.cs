using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class Player_TopDown : Moving
{
    [SerializeField]
    private PlayerHealth healthBar;
    [SerializeField]
    private GameObject fadingScreen;

    private Animator animator;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();

        PlayerPrefs.SetInt("player_max_health", maxHealthPoints);
        healthPoints = PlayerPrefs.GetInt("player_health", maxHealthPoints);
    }

    private void FixedUpdate()
    {
        float xVelocity = Input.GetAxisRaw("Horizontal");
        float yVelocity = Input.GetAxisRaw("Vertical");

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
        // To load menu after results page
        PlayerPrefs.SetInt("next_level", 1);
        // To not earn money of current room
        PlayerPrefs.GetInt("save_money", 0);
        // Animation of player death
        animator.SetBool("isAlive", isAlive);

        yield return new WaitForSeconds(1f);

        // Fading screen after death
        fadingScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        // Load results scene
        SceneManager.LoadScene(11);
    }
}
