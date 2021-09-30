using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject[] cameras;

    private static int selectedCamera;

    private void Start()
    {
        // Set state from the beginning, because it's static
        selectedCamera = 0;

        disableAllCameras();

        // Enable the first camera
        cameras[selectedCamera].SetActive(true);
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
}
