using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    [SerializeField]
    private float delay;

    private void Start()
    {
        StartCoroutine(ShowForSomeTime());
    }

    private IEnumerator ShowForSomeTime()
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(delay);

        // Hide after delay
        Destroy(gameObject);

        Time.timeScale = 1;
    }
}
