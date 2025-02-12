using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public IDCard IDCard;

    public MemberCard memberCard;

    public Image visitorImage;

    private void Start()
    {
        //IDCard = GetComponentInChildren<IDCard>();
    }

    public void SetupVisitor(Visitor visitor)
    {
        visitorImage.sprite = visitor.icon;
        IDCard.SetPhoto(visitor.icon);
        IDCard.SetIDInfo($"{visitor.firstName} {visitor.lastName}",
            visitor.age.ToString(), "Champion", visitor.profession);

        memberCard.SetImage(visitor.icon);
        memberCard.SetNameText(visitor.Name);
        memberCard.SetDateText(visitor.expirationDate);
        memberCard.SetIDText(visitor.id);
        //Visitor dumps all his ID and Membership card on table

        //Player asks visit what his preferences are

        //Visitor tells player his preferences

        //Player assigns a room key to them IF they are a member

        //Next visitor comes in
        //Rinse and Repeat

    }
}
