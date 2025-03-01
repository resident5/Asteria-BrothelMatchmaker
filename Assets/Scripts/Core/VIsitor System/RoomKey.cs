using UnityEngine;

public class RoomKey : DragSource
{
    //The room this key corresponds to
    public KeyEnum keyValue;
    public int MAX_USAGES = 2;
    public int numOfUsages = 2;

    //Get the GameManager
    //Set the current visitor to the room that corresponds to this key.
    //Move on to the next visitor

    private void Start()
    {
        base.Start();
        numOfUsages = MAX_USAGES;
    }

    public override void Action()
    {
        Debug.Log($"{keyValue.ToString()}");
        numOfUsages--;

        if (numOfUsages <= 0)
        {
            Debug.Log("Instantiate a new gameObject");
            GameObject obj = Instantiate(new GameObject(), initalParent);
            obj.transform.SetSiblingIndex(transform.GetSiblingIndex());
            transform.SetParent(initalParent);

            gameObject.SetActive(false);
        }

        GameManager.Instance.SendVisitorToRoom(keyValue);

        GameManager.Instance.SetNextVisitor();
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

