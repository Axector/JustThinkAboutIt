using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLastCutscene : CollidableObject
{
    [SerializeField]
    private GameObject dialogBackGround;
    [SerializeField]
    private GameObject[] texts;
    [SerializeField]
    private Animator npc;

    protected override void OnCollision(Collider2D other)
    {
        bool secondChapterFinished = PlayerPrefs.GetInt("second_chapter_beaten", 0) == 0;

        if (other.name == "Player" && secondChapterFinished) {
            StartCoroutine(ShowDialog());
        }
    }

    private IEnumerator ShowDialog()
    {
        // Stop game for dialog
        Time.timeScale = 0;
        dialogBackGround.SetActive(true);

        // Show whole dialog
        foreach (GameObject text in texts) {
            text.SetActive(true);

            yield return new WaitForSecondsRealtime(4f);

            text.SetActive(false);
        }

        dialogBackGround.SetActive(false);

        // Resume game after dialog
        Time.timeScale = 1;

        // Play animation of npc goes away
        npc.Play("GoAway");
    }
}
