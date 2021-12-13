using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadeIn : DefaultClass
{
    [SerializeField]
    protected float fadeInSpeed = 0.0005f;
    [SerializeField]
    protected float goUpSpeed;
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
        if (startFading)
        {
            // Decrease alpha to make the object fade out
            Color color = text.color;
            color.a += fadeInSpeed;
            text.color = color;

            // Destroy invisible object
            if (color.a >= 1) {
                startFading = false;
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
