using System.Collections;
using UnityEngine;

public class NPC_Fairy : NPC
{
    [SerializeField]
    private GameObject textBg;
    [SerializeField]
    private GameObject[] dialog;
    [SerializeField]
    private GameObject[] otherTexts;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private GameObject dog;
    [SerializeField]
    private GameObject blackScreenForBlink;
    [SerializeField]
    private int showDogTextIndex;
    [SerializeField]
    private int specialTriggerTextIndex;

    private void OnEnable()
    {
        StartCoroutine(StartDialog());
    }

    private IEnumerator StartDialog()
    {
        DisableOtherTexts();

        yield return new WaitForSecondsRealtime(2f);

        textBg.SetActive(true);

        // Show whole speech
        for (int i = 0; i < dialog.Length; i++) { 
            // Hide previous dialog text
            if (i > 0) {
                dialog[i - 1].SetActive(false);
            }

            // Show next dialog statement
            dialog[i].SetActive(true);

            // Wait for next text
            yield return new WaitForSecondsRealtime(4f);

            // Show dog NPC
            if (i == showDogTextIndex) {
                dog.SetActive(true);
            }

            if (i == specialTriggerTextIndex) {
                blackScreenForBlink.SetActive(true);
                dog.SetActive(false);

                yield return new WaitForSecondsRealtime(0.5f);

                blackScreenForBlink.SetActive(false);

                yield return new WaitForSecondsRealtime(1f);

                sprite.flipX = true;

                yield return new WaitForSecondsRealtime(1f);

                sprite.flipX = false;

                yield return new WaitForSecondsRealtime(1.5f);
            }
        }

        textBg.SetActive(false);
        speechEnd = true;
    }

    private void DisableOtherTexts()
    {
        foreach (GameObject text in otherTexts) {
            text.SetActive(false);
        }
    }
}
