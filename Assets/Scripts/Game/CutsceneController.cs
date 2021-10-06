using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    private enum Direction
    {
        left,
        right
    };

    [SerializeField]
    private CutscenePlayer player;
    [SerializeField]
    private Direction moveDirection = Direction.right;
    [SerializeField]
    private float targetPositionX = -16.63f;
    [SerializeField]
    private float bannerMoveSpeed = 20f;
    [SerializeField]
    private float delayBeforeStart = 1f;

    [SerializeField]
    private GameObject topBanner;
    [SerializeField]
    private GameObject bottomBanner;
    [SerializeField]
    private GameObject fadeScreen;

    private SpriteRenderer pSpriteRenderer;
    private Transform pTransform;
    private Animator pAnimator;
    private Image fadeScreenImage;

    private float velocityX;
    private float movementSpeed;
    private float fadeOutSpeed = 0.0015f;
    private bool startFading = false;
    private bool cutSceneStart = false;
    private bool moveCutsceneBanners = false;

    private void Start()
    {
        // Get different components of a player
        pSpriteRenderer = player.GetComponent<SpriteRenderer>();
        pTransform = player.GetComponent<Transform>();
        pAnimator = player.GetComponent<Animator>();
        fadeScreenImage = fadeScreen.GetComponent<Image>();

        // Set basic stats
        movementSpeed = player.PlayerSpeed;
        fadeScreenImage.color = Color.black;

        StartCoroutine(startCutScene());
    }

    private void Update()
    {
        if (cutSceneStart) {
            // Move right/left
            velocityX = (moveDirection == Direction.right) ? 1 : -1;

            // Stop when player reaches the target position
            if (pTransform.position.x >= targetPositionX) {
                velocityX = 0;
            }

            // Start move cutscene banners before player stops
            if (pTransform.position.x >= targetPositionX - 6) {
                StartCoroutine(endCutScene());
            }
        }

        // Smooth fade out to introduce the level
        if (startFading) {
            // Decrease alpha to make the object fade out
            Color color = fadeScreenImage.color;
            color.a -= fadeOutSpeed;
            fadeScreenImage.color = color;

            if (color.a <= 0) {
                startFading = false;
                Destroy(fadeScreen.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        // Movement to left and right while player is alive
        if (player.IsAlive) {
            Movement();

            // Set parameter for animator to animate running
            pAnimator.SetFloat("velocity", Mathf.Abs(velocityX));
        }

        if (moveCutsceneBanners) {
            topBanner.transform.position += Vector3.up * bannerMoveSpeed * Time.fixedDeltaTime;
            bottomBanner.transform.position += Vector3.down * bannerMoveSpeed * Time.fixedDeltaTime;
        }
    }

    private void Movement()
    {
        pTransform.position += new Vector3(velocityX * movementSpeed * Time.fixedDeltaTime, 0, 0);

        // Player sprite rotation (left/right)
        // When velocity == 0, should stay in last rotation
        if (velocityX < 0) {
            pSpriteRenderer.flipX = true;
        }
        else if (velocityX > 0) {
            pSpriteRenderer.flipX = false;
        }
    }

    private IEnumerator endCutScene()
    {
        moveCutsceneBanners = true;

        yield return new WaitForSeconds(3f);

        PlayerPrefs.SetFloat("CameraOrthographicSize", 7f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator startCutScene()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        startFading = true;
        cutSceneStart = true;
    }
}