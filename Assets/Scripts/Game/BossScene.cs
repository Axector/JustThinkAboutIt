using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using UnityEngine;

public class BossScene : DefaultClass
{
    [SerializeField]
    private AEnemy boss;
    [SerializeField]
    private GameObject agressionArea;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private GameObject sideEnemies;
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

    private bool bToggle = true;

    private void Start()
    {
        StartCoroutine(StartScene());

        // Set scene color
        PostProcessVolume postProcessing = cam.GetComponent<PostProcessVolume>();
        ColorGrading colorGrading;
        postProcessing.profile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = cameraController.SceneColor;

        PlayerPrefs.SetInt("next_level", nextLevelIndex);
    }

    private void FixedUpdate()
    {
        // If boss died
        if (boss && !boss.IsAlive && bToggle) {
            bToggle = false;

            // Hide blocker
            blocker.isTrigger = true;
            ParticleSystem.MainModule blockerMain = blockerParticles.main;
            blockerMain.loop = false;

            // Open exit to next level
            exit.SetActive(true);
        }

        if (!boss && timer.bTimerEnded && bToggle) {
            bToggle = false;

            // Hide blocker
            blocker.isTrigger = true;
            ParticleSystem.MainModule blockerMain = blockerParticles.main;
            blockerMain.loop = false;
            sideEnemies.SetActive(false);

            // Open exit to next level
            exit.SetActive(true);
        }
    }

    private IEnumerator StartScene()
    {
        yield return new WaitForSeconds(2f);

        // Show the boss
        cameraController.SelectNextCamera();
        cam.Play("EnterBossScene");

        yield return new WaitForSeconds(7.5f);

        // Resume the game
        cameraController.SelectNextCamera();
        if (agressionArea) {
            agressionArea.SetActive(true);
        }
        player.enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
        Destroy(cam.gameObject);
    }
}
