using UnityEngine.UI;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private Text moneyEarnedText;
    [SerializeField]
    private string beforeText;
    [SerializeField]
    private Color color;

    private GameObject alertParent;
    private CameraController cameraController;

    private void Awake()
    {
        alertParent = GameObject.FindGameObjectWithTag("AlertParent");
        cameraController = FindObjectOfType<CameraController>();
    }

    public void showPopup(string text)
    {

        // Get position in viewport
        Vector2 newPosition = cameraController.getActiveCamera().WorldToViewportPoint(transform.position);

        // Translate coordinates to the screen
        newPosition = new Vector2(newPosition.x * Screen.width, newPosition.y * Screen.height);

        // Create text object on the screen
        Text alert = Instantiate(
            moneyEarnedText,
            Vector2.zero,
            Quaternion.identity,
            alertParent.transform
        );

        // Change some settings for text object
        alert.transform.localPosition = newPosition;
        alert.text = beforeText + text;
        alert.color = color;

        // Fade out and destroy text object
        alert.GetComponent<FadeOut>().fadeOut();
    }
}
