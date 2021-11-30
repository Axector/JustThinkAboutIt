using UnityEngine.UI;
using UnityEngine;

public class ShowHelpText : DefaultClass
{
    [SerializeField]
    private Text helpText;

    private void OnTriggerStay2D(Collider2D collision)
    {
        helpText.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        helpText.enabled = false;
    }
}
