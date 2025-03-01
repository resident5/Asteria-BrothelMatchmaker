using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDoor : MonoBehaviour
{
    [SerializeField] private Image visitor1Image;
    [SerializeField] private Image visitor2Image;

    [SerializeField] private TMP_Text scoreText;

    public void SetRoomInfo(Room roomData)
    {
        visitor1Image.sprite = roomData.visitor1.idIcon;

        if(roomData.visitor2 == null)
        {
            visitor2Image.gameObject.SetActive(false);
        }
        else
        {
            visitor2Image.sprite = roomData.visitor2.idIcon;
        }

        scoreText.text = roomData.GetFinalRoomScore().ToString();
    }
}
