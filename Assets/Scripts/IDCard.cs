using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IDCard : MonoBehaviour
{
    public Image profileIcon;

    public TMP_Text profileName;
    public TMP_Text profileAge;
    public TMP_Text profileClass;
    public TMP_Text profileJob;

    public void SetPhoto(Sprite sprite)
    {
        profileIcon.sprite = sprite;
    }

    public void SetIDInfo(string name, string age, string className, string jobName)
    {
        profileName.text = name;
        profileAge.text = age;
        profileClass.text = className;
        profileJob.text = jobName;
    }
}
