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
        if (NearlyEqual(rectTransform.localPosition.y, targetPosition.y, speed / 10)) {
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
