using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private Image image;

    public void SetMember(Visitor member)
    {
        nameText.text = member.Name;
        idText.text = member.id;
        image.sprite = member.idIcon;
    }   
}
