using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableContainer : MonoBehaviour
{
    public GameObject id;
    public GameObject memberId;

    public void ShowId()
    {
        id.SetActive(true);
    }

    public void ShowMemberID()
    {
        memberId.SetActive(true);
    }

    public void HideID()
    {
        id.SetActive(false);
    }

    public void HideMemberID()
    {
        memberId.SetActive(false);
    }
}
