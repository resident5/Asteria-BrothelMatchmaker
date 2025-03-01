using System;
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

    public void SetNameColor(Color color) => nameText.color = color;

    public void SetNameFont(TMP_FontAsset font) => nameText.font = font;

}
