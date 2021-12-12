using UnityEngine.UI;
using UnityEngine;

public class ShopButton : DefaultClass
{
    [SerializeField]
    private int cost;
    [SerializeField]
    private string code;

    public Button button;

    public int Cost { get => cost; }
    public string Code { get => code; }

    private void Start()
    {
        button.interactable = PlayerPrefs.GetInt(code, 0) != 1;
    }
}
