using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cameras;
    [SerializeField]
    private float rightEdge;
    [SerializeField]
    private float leftEdge;
    [SerializeField]
    private float upEdge;
    [SerializeField]
    private float downEdge;

    private static int selectedCamera;

    private Player player;
    private CameraController cameraController;
    private Camera activeCamera;

    private bool left;
    private bool right;
    private bool up;
    private bool down;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        cameraController = FindObjectOfType<CameraController>();
        activeCamera = cameraController.getActiveCamera();

        // Set state from the beginning, because it's static
        selectedCamera = 0;

        disableAllCameras();

        // Enable the first camera
        cameras[selectedCamera].SetActive(true);
    }

    private void Update()
    {
        // Check player position on the screen
        Vector2 playerPosition = activeCamera.WorldToViewportPoint(player.transform.position);

        Debug.Log(playerPosition);

        right = playerPosition.x > rightEdge;
        left = playerPosition.x < leftEdge;
        up = playerPosition.y > upEdge;
        down = playerPosition.y < downEdge;
    }

    private void FixedUpdate()
    {
        // Camera movement if player is near the edge of screen
        if (right) {
            MoveCamera(Vector3.right);
        }

        if (left) {
            MoveCamera(-Vector3.right);
        }

        if (up) {
            MoveCamera(Vector3.up);
        }

        if (down) {
            MoveCamera(-Vector3.up);
        }
    }

    private void MoveCamera(Vector3 direction)
    {
        // Move only if player is alive
        if (player.IsAlive) { 
            activeCamera.transform.position += direction * player.PlayerSpeed * Time.fixedDeltaTime;
        }
    }

    private void disableAllCameras()
    {
        foreach (GameObject camera in cameras) {
            camera.SetActive(false);
        }
    }

    public void selectNextCamera()
    {
        // If an array with cameras is not empty
        if(cameras.Length > 0)
        {
            // Disable current camera
            cameras[selectedCamera].SetActive(false);
            
            // Increase index to select next camera
            selectedCamera++;

            // If the first camera should be selected again
            if (selectedCamera >= cameras.Length) {
                selectedCamera = 0;
            }

            // Enable next camera in an array
            cameras[selectedCamera].SetActive(true);
        }
    }

    public Camera getActiveCamera()
    {
        return cameras[selectedCamera].GetComponent<Camera>();
    }
}
