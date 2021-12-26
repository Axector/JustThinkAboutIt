using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadeOut : DefaultClass
{
    [SerializeField]
    protected float fadeOutSpeed = 0.0005f;
    [SerializeField]
    protected float goUpSpeed = 0.1f;
    [SerializeField]
    private float delay;
    [SerializeField]
    private bool bHidden;

    private bool startFading = false;
    private Text text;

    protected virtual void Awake()
    {
        text = GetComponent<Text>();

        if (bHidden) {
            Color color = text.color;
            color.a = 0;
            text.color = color;
        }

        StartCoroutine(DelayBeforeFading());
    }

    private void Update()
    {
        if (startFading) {
            // Decrease alpha to make the object fade out
            Color color = text.color;
            color.a -= fadeOutSpeed * 3;
            text.color = color;

            // Destroy invisible object
            if (color.a <= 0) {
                Destroy(gameObject);
            }
        }

        // Slowly move text up
        transform.position += Vector3.up * goUpSpeed;
    }

    private IEnumerator DelayBeforeFading()
    {
        yield return new WaitForSeconds(delay);

        if (bHidden) {
            Color color = text.color;
            color.a = 1f;
            text.color = color;
        }

        startFading = true;
    }
}
