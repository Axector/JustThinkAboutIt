using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using UnityEngine;

public class MinesTriplets : DefaultClass
{
    [SerializeField]
    private GameObject playerController;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private GameObject sideEnemies;
    [SerializeField]
    private GameObject moreEnemies;
    [SerializeField]
    private GameObject healthItem;
    [SerializeField]
    private GameObject dropPosition;
    [SerializeField]
    private BoxCollider2D player;
    [SerializeField]
    private BoxCollider2D blocker;
    [SerializeField]
    private ParticleSystem blockerParticles;
    [SerializeField]
    private Animator cam;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private GameObject exit;
    [SerializeField]
    private int nextLevelIndex;

    private bool bToggleExit = true;
    private bool bToggleHalf = true;

    private void Start()
    {
        // Set scene color
        PostProcessVolume postProcessing = cam.GetComponent<PostProcessVolume>();
        ColorGrading colorGrading;
        postProcessing.profile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = cameraController.SceneColor;

        StartCoroutine(StartScene());

        PlayerPrefs.SetInt("next_level", nextLevelIndex);
    }

    private void Update()
    {
        if (timer.bTimerEnded && bToggleExit) {
            bToggleExit = false;

            // Hide blockers
            blocker.isTrigger = true;
            ParticleSystem.MainModule blockerMain = blockerParticles.main;
            blockerMain.loop = false;
            sideEnemies.SetActive(false);

            // Open exit to next level
            exit.SetActive(true);
        }

        if (timer.bTimerHalf && bToggleHalf) {
            bToggleHalf = false;

            moreEnemies.SetActive(true);
            StartCoroutine(DropHealth());
        }
    }

    private IEnumerator StartScene()
    {
        yield return new WaitForSeconds(0.1f);

        // Stop player controller
        playerController.SetActive(false);

        yield return new WaitForSeconds(1.9f);

        cameraController.SelectNextCamera();
        cam.Play("EnterBossScene");

        yield return new WaitForSeconds(6.5f);

        playerController.SetActive(true);

        yield return new WaitForSeconds(1f);

        // Resume the game on cutscene end
        cameraController.SelectNextCamera();
        player.enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;

        Destroy(cam.gameObject);
    }

    private IEnumerator DropHealth()
    {
        for (int i = 0; i < 3; i++) {
            Instantiate(
                healthItem,
                dropPosition.transform.position,
                Quaternion.identity
            );

            yield return new WaitForSeconds(1f);
        }
    }
}
