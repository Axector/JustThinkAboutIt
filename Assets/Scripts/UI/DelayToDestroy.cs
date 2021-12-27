using System.Collections;
using UnityEngine;

public class DelayToDestroy : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeAppear;
    [SerializeField]
    private float delayBeforeDestroy;
    [SerializeField]
    private GameObject thatObject;

    private void Start()
    {
        StartCoroutine(DelayBeforeDestory());
    }

    private IEnumerator DelayBeforeDestory()
    {
        yield return new WaitForSeconds(delayBeforeAppear);

        thatObject.SetActive(true);

        yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(thatObject);
    }
}
