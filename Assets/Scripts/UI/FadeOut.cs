using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadeOut : DefaultClass
{
    [SerializeField]
    private float fadeOutSpeed = 0.0005f;
    [SerializeField]
    private float goUpSpeed = 0.1f;

    private bool startFading = false;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (startFading) {
            // Decrease alpha to make the object fade out
            Color color = text.color;
            color.a -= fadeOutSpeed;
            text.color = color;
        }

        transform.position += Vector3.up * goUpSpeed;
    }

    public void StartFadeOut()
    {
        StartCoroutine(DestroyAfterFadeOut());
    }

    private IEnumerator DestroyAfterFadeOut()
    {
        startFading = true;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
