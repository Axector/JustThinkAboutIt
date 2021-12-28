using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class CameraController : DefaultClass
{
    [SerializeField]
    private GameObject[] cameras;
    [SerializeField]
    private Color sceneColor;
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
    private float bonusSpeed;
    private bool left;
    private bool right;
    private bool up;
    private bool down;
    private bool upCameraSize;
    private bool downCameraSize;
    private PostProcessVolume postProcessing;

    public Color SceneColor { get => sceneColor; }

    private void Start()
    {
        DisableAllCameras();

        // Enable the first camera
        cameras[selectedCamera].SetActive(true);

        // Get player and active camera
        player = FindObjectOfType<Player>();
        activeCamera = GetActiveCamera();

        // Camera settings
        if (maxSize != 0) {
            // Set scene color
            postProcessing = cameras[selectedCamera].GetComponent<PostProcessVolume>();
            ColorGrading colorGrading;
            postProcessing.profile.TryGetSettings(out colorGrading);
            colorGrading.colorFilter.value = sceneColor;

            activeCamera.orthographicSize = PlayerPrefs.GetFloat("camera_size", 7f);
        }
    }

    private void Update()
    {
        if (maxSize == 0) {
            return;
        }

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

        // To always see player additional speed is needed
        bonusSpeed = (
            playerPosition.x > 0.9f ||
            playerPosition.x < 0.1f ||
            playerPosition.y > 0.9f ||
            playerPosition.y < 0.1f
        ) 
            ? 3f
            : 1f;
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
            activeCamera.transform.position += direction * player.PlayerSpeed * bonusSpeed * Time.fixedDeltaTime;
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
        if (downCameraSize && size <= maxSize) {
            size += cameraSizeChangeSpeed;
        }

        // Reduce camera size
        if (upCameraSize && size >= minSize) {
            size -= cameraSizeChangeSpeed;
        }

        // Set camera size
        activeCamera.orthographicSize = size;
        PlayerPrefs.SetFloat("camera_size", size);
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
