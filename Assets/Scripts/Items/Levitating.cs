using UnityEngine;

public class Levitating : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float amplitude = 1f;

    private float y0;

    private void Start()
    {
        // Starting height
        y0 = transform.position.y;
    }

    private void Update()
    {
        // Target height where to move
        float y1 = y0 + amplitude * Mathf.Sin(speed * Time.time);

        // Change only y position
        transform.position = new Vector3(
            transform.position.x,
            y1,
            transform.position.z
        );
    }
}
