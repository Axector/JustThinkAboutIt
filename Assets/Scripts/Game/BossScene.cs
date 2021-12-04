using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BossScene : DefaultClass
{
    [SerializeField]
    private AEnemy boss;
    [SerializeField]
    private GameObject agressionArea;
    [SerializeField]
    private GameObject bossHealthBar;
    [SerializeField]
    private Image bossHealthBarFill;
    [SerializeField]
    private Text bossHealthBarText;
    [SerializeField]
    private GameObject playerController;
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
    [SerializeField]
    private bool bLastLevel;
    [SerializeField]
    private GameObject lastFadingScreen;
    [SerializeField]
    private KeyObjectDestroy keyObject;

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

            // Hide blockers and boss health bar
            blocker.isTrigger = true;
            ParticleSystem.MainModule blockerMain = blockerParticles.main;
            blockerMain.loop = false;
            bossHealthBar.SetActive(false);

            // Open exit to next level
            exit.SetActive(true);
        }

        // Change boss health bar status
        if (boss) {
            ShowBossHealth();
        }

        if (timer && timer.bTimerEnded && bToggle) {
            bToggle = false;

            // Hide blockers
            blocker.isTrigger = true;
            ParticleSystem.MainModule blockerMain = blockerParticles.main;
            blockerMain.loop = false;
            sideEnemies.SetActive(false);

            // Open exit to next level
            exit.SetActive(true);
        }

        if (keyObject && bToggle && keyObject.IsDestroyed) {
            bToggle = false;

            StartCoroutine(EndLastLevel());
        }
    }

    private IEnumerator StartScene()
    {
        yield return new WaitForSeconds(0.1f);

        // Stop player controller
        playerController.SetActive(false);

        yield return new WaitForSeconds(1.9f);

        // Show the boss
        cameraController.SelectNextCamera();
        if (bLastLevel) {
            cam.Play("EnterLastBossScene");
        }
        else {
            cam.Play("EnterBossScene");
        }

        yield return new WaitForSeconds(2f);

        // Show boss health bar
        if (boss) {
            bossHealthBar.SetActive(true);
        }

        yield return new WaitForSeconds(5.5f);

        // Resume the game
        cameraController.SelectNextCamera();
        playerController.SetActive(true);

        if (boss) {
            agressionArea.SetActive(true);
        }
        else {
            player.enabled = true;
            player.GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        Destroy(cam.gameObject);
    }

    private IEnumerator EndLastLevel()
    {
        yield return new WaitForSeconds(4f);

        lastFadingScreen.SetActive(true);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(10);
    }

    private void ShowBossHealth()
    {
        int bossHealth = boss.Health;

        bossHealthBarFill.fillAmount = (float)bossHealth / boss.MaxHealth;
        bossHealthBarText.text = bossHealth.ToString();
    }
}
