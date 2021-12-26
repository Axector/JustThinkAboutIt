using UnityEngine.Rendering.PostProcessing;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MotherOfSpheres : DefaultClass
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
    private SpriteRenderer[] objectsToHide;
    [SerializeField]
    private Animator[] topClouds;
    [SerializeField]
    private GameObject playerController;
    [SerializeField]
    private BoxCollider2D blocker;
    [SerializeField]
    private ParticleSystem blockerParticles;
    [SerializeField]
    private GameObject randomExplosionMark;
    [SerializeField]
    private ParticleSystem randomExplosion;
    [SerializeField]
    private Animator cam;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private GameObject exit;
    [SerializeField]
    private int nextLevelIndex;

    private bool bToggleExit = true;
    private bool bToggleSecondPhase = true;
    private float delayBeforeNextExplosion = 4f;

    private void Start()
    {
        // Set scene color
        PostProcessVolume postProcessing = cam.GetComponent<PostProcessVolume>();
        ColorGrading colorGrading;
        postProcessing.profile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = cameraController.SceneColor;

        StartCoroutine(StartScene());
        StartCoroutine(StartRandomExplosion());

        PlayerPrefs.SetInt("next_level", nextLevelIndex);
    }

    private void Update()
    {
        if (!boss.IsAlive && bToggleExit) {
            bToggleExit = false;

            // Hide blockers and boss health bar
            blocker.isTrigger = true;
            ParticleSystem.MainModule blockerMain = blockerParticles.main;
            blockerMain.loop = false;
            bossHealthBar.SetActive(false);

            // Open exit to next level
            exit.SetActive(true);
        }

        // Change boss health bar status
        ShowBossHealth();

        // Change boss behavior on half of his health
        if (boss.Health < boss.MaxHealth / 2) {
            HideObjects();

            if (bToggleSecondPhase) {
                bToggleSecondPhase = false;

                ChangeCloudMovement();
                delayBeforeNextExplosion -= 1f;
            }
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
        cam.Play("EnterBossScene");

        yield return new WaitForSeconds(2f);

        // Show boss health bar
        bossHealthBar.SetActive(true);

        yield return new WaitForSeconds(5.5f);

        // Resume the game
        cameraController.SelectNextCamera();
        playerController.SetActive(true);

        // Begin boss attacks
        agressionArea.SetActive(true);

        Destroy(cam.gameObject);
    }

    private IEnumerator StartRandomExplosion()
    {
        yield return new WaitForSecondsRealtime(10f);

        while (boss.IsAlive) {
            float xPos = Random.Range(-23f, -8f);
            float yPos = Random.Range(-2f, 3f);
            Vector2 position = new Vector2(xPos, yPos);

            GameObject mark = Instantiate(
                randomExplosionMark,
                position,
                Quaternion.identity
            );

            yield return new WaitForSeconds(1f);

            Destroy(mark);

            Instantiate(
                randomExplosion,
                position,
                Quaternion.identity
            );

            yield return new WaitForSeconds(delayBeforeNextExplosion);
        }
    }

    private void ShowBossHealth()
    {
        int bossHealth = boss.Health;

        bossHealthBarFill.fillAmount = (float)bossHealth / boss.MaxHealth;
        bossHealthBarText.text = bossHealth.ToString();
    }

    private void HideObjects()
    {
        foreach (SpriteRenderer sprite in objectsToHide) {
            // Decrease alpha to make the object fade out
            Color color = sprite.color;
            color.a -= Time.deltaTime;
            sprite.color = color;

            // Destroy invisible object
            if (color.a <= 0) {
                sprite.gameObject.SetActive(false);
            }
        }
    }

    private void ChangeCloudMovement()
    {
        foreach (Animator cloud in topClouds) {
            cloud.GetComponent<Levitating>().enabled = false;
            cloud.Play("StartHorizontalMovement");
        }
    }
}
