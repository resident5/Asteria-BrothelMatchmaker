using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MemberCard : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    TMP_Text nameText, IDText, dateText;

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetNameText(string name)
    {
        nameText.text = name;
    }

    public void SetIDText(string id)
    {
        IDText.text = id;
    }

    public void SetDateText(Date date)
    {
        dateText.text = date.ToString();
    }
}
