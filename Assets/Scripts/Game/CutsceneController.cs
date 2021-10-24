using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
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
    private int sceneIndexToLoad;
    [SerializeField]
    private Color sceneColor;
    [SerializeField]
    private CutscenePlayer player;
    [SerializeField]
    private Camera cam;
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
    private PostProcessVolume postProcessing;

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
        postProcessing = cam.GetComponent<PostProcessVolume>();

        // Set basic stats
        movementSpeed = player.PlayerSpeed;
        fadeScreenImage.color = Color.black;
        cam.orthographicSize = 14f;

        // Set scene color
        ColorGrading colorGrading;
        postProcessing.profile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = sceneColor;

        StartCoroutine(StartCutScene());
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
                StartCoroutine(EndCutScene());
            }
        }

        // Smooth fade out to introduce the level
        if (startFading) {
            // Decrease alpha to make the fadeScreen fade out
            Color color = fadeScreenImage.color;
            color.a -= fadeOutSpeed;
            fadeScreenImage.color = color;

            // Camera flight to the player
            if (cam.orthographicSize > 7f) {
                cam.orthographicSize -= fadeOutSpeed * 10;
            }
            else {
                cam.orthographicSize = 7f;
            }

            // When fadeScreen is invisible, destroy its game object
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

        // Move top banner up and bottom banner down
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

    private IEnumerator EndCutScene()
    {
        // Start to move top and bottom black banners
        moveCutsceneBanners = true;

        yield return new WaitForSeconds(3f);

        // Set camera size and load game scene
        PlayerPrefs.SetFloat("CameraOrthographicSize", 7f);
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    private IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        // Start cutscene animations
        startFading = true;
        cutSceneStart = true;
    }
}