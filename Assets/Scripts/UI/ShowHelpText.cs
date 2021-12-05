using UnityEngine.UI;
using UnityEngine;

public class ShowHelpText : DefaultClass
{
    [SerializeField]
    private Text helpText;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            helpText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            helpText.enabled = false;
        }
    }
}
