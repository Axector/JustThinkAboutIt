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
    [SerializeField]
    private float maxSize = 9f;
    [SerializeField]
    private float minSize = 5f;
    [SerializeField]
    private float cameraSizeChangeSpeed = 0.1f;

    private Player player;
    private Camera activeCamera;

    private int selectedCamera = 0;
    private bool left;
    private bool right;
    private bool up;
    private bool down;
    private bool upCameraSize;
    private bool downCameraSize;

    private void Start()
    {
        DisableAllCameras();

        // Enable the first camera
        cameras[selectedCamera].SetActive(true);

        // Get player and active camera
        player = FindObjectOfType<Player>();
        activeCamera = GetActiveCamera();

        // Set default size to camera
        activeCamera.orthographicSize = Settings.GetCameraOrthographicSize();
    }

    private void Update()
    {
        // Check player position on the viewport
        Vector2 playerPosition = activeCamera.WorldToViewportPoint(player.transform.position);

        // Check if camera must move and the direction of that movement
        right = playerPosition.x > rightEdge;
        left = playerPosition.x < leftEdge;
        up = playerPosition.y > upEdge;
        down = playerPosition.y < downEdge;

        // Check to change camera size or not
        upCameraSize = (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals));
        downCameraSize = Input.GetKey(KeyCode.Minus);
    }

    private void FixedUpdate()
    {
        checkMoveCamera();

        // If any of resize buttons is pressed
        if (upCameraSize || downCameraSize) { 
            ChangeCameraSize();
        }
    }

    private void checkMoveCamera()
    {
        // Camera movement if player is near the edge of specific area
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

    private void DisableAllCameras()
    {
        foreach (GameObject camera in cameras) {
            camera.SetActive(false);
        }
    }

    private void ChangeCameraSize()
    {
        // Get current camera size
        float size = activeCamera.orthographicSize;

        // Increase camera size
        if (upCameraSize && size <= maxSize) {
            size += cameraSizeChangeSpeed;
        }

        // Reduce camera size
        if (downCameraSize && size >= minSize) {
            size -= cameraSizeChangeSpeed;
        }

        // Set camera size and save value
        activeCamera.orthographicSize = size;
        Settings.SetCameraOrthographicSize(size);
    }

    public void SelectNextCamera()
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
            activeCamera = GetActiveCamera();
        }
    }

    public Camera GetActiveCamera()
    {
        return cameras[selectedCamera].GetComponent<Camera>();
    }
}
