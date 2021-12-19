using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadingScreen : MonoBehaviour
{
    [SerializeField]
    private float fadingSpeed;
    [SerializeField]
    private bool toFadeIn;
    [SerializeField]
    private float delay;

    private void OnEnable()
    {
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        yield return new WaitForSeconds(delay);

        Image image = GetComponent<Image>();

        while ((toFadeIn) ? image.color.a < 1f : image.color.a > 0f) {
            Color color = image.color;
            color.a += (toFadeIn) ? 5f/255f : -5f/255f;
            image.color = color;

            yield return new WaitForSecondsRealtime(1f / fadingSpeed);
        }

        if (!toFadeIn) {
            gameObject.SetActive(false);
        }
    }
}
