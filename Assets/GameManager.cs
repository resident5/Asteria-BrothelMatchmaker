using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public HUDController hudController;

    public Visitor currentVisitor;

    public List<VisitorSO> visitorObjects;

    //List of members that are supposed to be in
    //These are 100% accurate members that are pulled directly from their SOs
    public List<VisitorSO> membersLists;
    public const int maxMembers = 1;

    public Room[] rooms;

    private void Awake()
    {
        hudController = GameObject.FindObjectOfType<HUDController>();
        
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }


        KeyEnum[] roomKeys = (KeyEnum[])System.Enum.GetValues(typeof(KeyEnum));
        rooms = new Room[roomKeys.Length - 1];

        for (int i = 0; i < roomKeys.Length - 1; i++)
        {
            rooms[i] = new Room(roomKeys[i]);
        }
    }

    private void Start()
    {
        membersLists = Resources.LoadAll<VisitorSO>("Visitors").ToList();

        InitializeGame();
        SetupVisitor();
    }

    public void InitializeGame()
    {
        List<Visitor> listOfMembers = GenerateMemberList();

    }
    
    private VisitorSO SelectRandomVisitor()
    {
        int randIndex = Random.Range(0, visitorObjects.Count);
        VisitorSO randomVisitor = visitorObjects[randIndex];
        visitorObjects.RemoveAt(randIndex);

        return randomVisitor;
    }

    public void SetupVisitor()
    {
        VisitorSO selectedVisitor = SelectRandomVisitor();
        //currentVisitor = new Visitor(testVisitor);

        hudController.SetupVisitor(selectedVisitor);
    }

    IEnumerator AssignVisitor()
    {
        yield return null;
    }

    private List<Visitor> GenerateMemberList()
    {
        List<Visitor> tempMembers = new List<Visitor>();
        for (int i = 0; i < maxMembers; i++)
        {
            int randMember = Random.Range(0, membersLists.Count);
            Visitor visitor = new Visitor(membersLists[randMember]);
            tempMembers.Add(visitor);
        }

        return tempMembers;
    }

    public void SendVisitorToRoom(KeyEnum draggedRoomKey)
    {
        foreach (var room in rooms)
        {
            if(room.roomKey == draggedRoomKey)
            {
                room.SetVisitor(currentVisitor);
            }
        }
    }
}
