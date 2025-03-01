using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MembersList : MonoBehaviour
{
    public Visitor[] members;
    public GameObject membersPrefab;
    public Transform holder;


    private void Start()
    {
        members = GameManager.Instance.GetMembers();
        foreach (var member in members)
        {
            GameObject obj = Instantiate(membersPrefab, holder);
            obj.GetComponent<MemberInfo>().SetMember(member);
        }
    }
}
