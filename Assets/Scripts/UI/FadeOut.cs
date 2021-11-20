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

    private bool startFading = false;
    private Text text;

    protected virtual void Awake()
    {
        text = GetComponent<Text>();

        StartCoroutine(DelayBeforeFading());
    }

    private void Update()
    {
        if (startFading) {
            // Decrease alpha to make the object fade out
            Color color = text.color;
            color.a -= fadeOutSpeed;
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

        startFading = true;
    }
}
