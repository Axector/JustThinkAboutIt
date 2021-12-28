using UnityEngine;

public class Player_TopDown : Moving
{
    [SerializeField]
    private PlayerHealth healthBar;

    private Animator animator;

    protected override void Start()
    {
        base.Start();

        //DEBUG
        PlayerPrefs.SetInt("player_money", 0);
        PlayerPrefs.SetInt("player_health", maxHealthPoints);

        animator = GetComponent<Animator>();

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
}
