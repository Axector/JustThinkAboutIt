using UnityEngine;

public class SlideTo : DefaultClass
{
    [SerializeField]
    Vector3 targetPosition;
    [SerializeField]
    float speed;

    private RectTransform rectTransform;
    private bool isSliding;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isSliding = true;
    }

    private void Update()
    {
        // Check current to target position distance
        if (NearlyEqual(rectTransform.localPosition, targetPosition, 0.5f)) {
            isSliding = false;
        }
    }

    private void FixedUpdate()
    {
        // Slide to position
        if (isSliding) {
            rectTransform.position += speed * Vector3.down * Time.fixedDeltaTime;
        }
    }
}
