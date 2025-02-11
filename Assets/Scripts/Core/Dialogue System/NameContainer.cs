using System;
using System.Collections;
using TMPro;
using UnityEngine;


[Serializable]
public class NameContainer
{
    [SerializeField]
    private GameObject root;
    
    [SerializeField]
    private TextMeshProUGUI nameText;


    public void Show(string speakerName = "")
    {
        root.SetActive(true);

        if (!string.IsNullOrEmpty(speakerName))
        {
            nameText.text = speakerName;
        }

    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
