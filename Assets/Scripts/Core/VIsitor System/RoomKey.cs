using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomKey : DragSource
{
    //The room this key corresponds to
    public KeyEnum keyValue;

    //Get the GameManager
    //Set the current visitor to the room that corresponds to this key.
    //Move on to the next visitor

    //Testing only

    public override void Action()
    {
        Debug.Log($"{keyValue.ToString()}");

        GameManager.Instance.SendVisitorToRoom(keyValue);

        //GameManager.Instance.SetNextVisitor();
    }

}



public enum KeyEnum
{
    Room1 = 1,
    Room2 = 2,
    Room3 = 3,
    Room4 = 4,
    Room5 = 5,
    Room6 = 6,
    NONE,
}

