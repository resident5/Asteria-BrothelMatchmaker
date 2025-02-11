using UnityEngine;

[System.Serializable]
public class Room
{
    public Visitor visitor1 = null;
    public Visitor visitor2 = null;

    public int matchScore;

    public KeyEnum roomKey;

    [SerializeField]
    private int baseScore = 100;

    public Room(KeyEnum key)
    {
        roomKey = key;
    }

    public void SetVisitor(Visitor visitor)
    {
        //Set Visitor Parameters
        //Allow player to check room info somewhere on screen
        if (visitor1 == null)
        {
            visitor1 = visitor;
            Debug.Log($"{visitor.firstName} {visitor.lastName} has been sent to {roomKey.ToString()}");
            return;
        }
        else if (visitor2 == null)
        {
            visitor2 = visitor;
            Debug.Log($"{visitor.firstName} {visitor.lastName} has been sent to {roomKey.ToString()}");
            return;
        }

        Debug.LogError("Room is full... This is an error");
    }

    /// <summary>
    /// Called at the end of the game when there's 0 visitors left in the queue to tally up the score of all the rooms
    /// </summary>
    /// <returns></returns>
    public int GetFinalRoomScore()
    {
        //Check if one of the visitors is a imposter
        //If they are final score is -100 and trigger a scene
        int finalScore = 0;

        //Compare Visitor1 and Visitor2's stats with the others preferred stats
        finalScore += visitor1.CompareStatTraits(visitor2);
        finalScore += visitor2.CompareStatTraits(visitor1);

        //Compare Visitor1 and Visitor2's stats with the others
        foreach (var trait in visitor2.stats)
        {
            if (visitor1.myPreferredTraits.Contains(trait))
            {
                finalScore += baseScore;
            }
        }

        foreach (var trait in visitor1.stats)
        {
            if (visitor1.myPreferredTraits.Contains(trait))
            {
                finalScore += baseScore;
            }
        }

        return finalScore;
    }
}
