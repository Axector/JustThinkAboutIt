using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfPlayerPrefsTrue : DefaultClass
{
    [SerializeField]
    private string code;
    [SerializeField]
    private int isTrue;

    private void Awake()
    {
        // Hide this object if given state is true
        if (PlayerPrefs.GetInt(code, 0) == isTrue) {
            gameObject.SetActive(false);
        }
    }
}
