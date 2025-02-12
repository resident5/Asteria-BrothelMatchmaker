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

    private List<VisitorSO> allCharacters;

    public List<Visitor> visitorsList;
    public Visitor currentVisitor;

    public const int MAX_VISITORS = 6;
    public const int MAX_MEMBERS = 2;

    //List of members that are supposed to be in
    //These are 100% accurate members that are pulled directly from their SOs
    public List<Visitor> membersList;
    public Room[] rooms;

    public Animator visitorAnimator;

    public const string VISITOR_ENTER_ANIM = "Entering";
    public const string VISITOR_EXIT_ANIM = "Exiting";

    public enum GameState
    {
        START, //Start of the game (Handle intro dialogue, tutorial, etc)
        PLAYING, //Current Game Loop where the player will be for most
        SCORING, //Scoring part at the end of the playing loop when theres no more visitors
        END //End screen where all of the sex scenes are added
    }

    public GameState gameState = GameState.START;

    //Create a list of ALL the characters that CAN show up (Resources.load)
    //Create a list of 6 potential characters that can show up (Members)
    //Create a list of all visitors that are going to show up in this game (Visitors)

    //Randomly choose between 4 to 6 members from the members list and put them in the visitors list
    //Randomly choose between 2 to 4 characters to put in the visitors list
    //Should be between 6 - 10 visitors in total

    //Randomly choose between 2-4 several characters in the entire visitor list to make set as imposters
    //Then make 1 random modification to that character (wrong name, wrong id, expy date, wrong icon etc)
    //Start game

    private void Awake()
    {
        hudController = GameObject.FindObjectOfType<HUDController>();

        if (Instance == null)
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
        InitializeGame();
    }

    private void Update()
    {
        //if (visitorsList.Count <= 0)
        //{
        //    Debug.Log("Game End");
        //}
    }
    public void InitializeGame()
    {
        allCharacters = Resources.LoadAll<VisitorSO>("Visitors").ToList();
        membersList = CreateMembersList();
        visitorsList = GetRandomNumberOfElements(membersList, 3);
        SelectVisitor();
        gameState = GameState.PLAYING;

    }

    public void SelectVisitor()
    {
        StartCoroutine(VisitorEntering());
    }

    public IEnumerator VisitorEntering()
    {

        currentVisitor = SelectRandomVisitor();
        hudController.SetupVisitor(currentVisitor);

        visitorAnimator.Play(VISITOR_ENTER_ANIM);

        yield return visitorAnimator.GetCurrentAnimatorStateInfo(0).IsName(VISITOR_ENTER_ANIM);

    }

    public IEnumerator VisitorExiting()
    {
        visitorAnimator.Play(VISITOR_EXIT_ANIM);

        yield return visitorAnimator.GetCurrentAnimatorStateInfo(0).IsName(VISITOR_EXIT_ANIM);

        if(visitorsList.Count != 0)
        {
            SelectVisitor();
        }
        else
        {
            //Go to scoreboard
        }
    }

    /// <summary>
    /// Returns a random list of members created from allcharacters list
    /// </summary>
    /// <returns></returns>
    private List<Visitor> CreateMembersList()
    {
        List<Visitor> tempMembers = new List<Visitor>();
        List<VisitorSO> tmpList = allCharacters.ToList();
        for (int i = 0; i < MAX_MEMBERS; i++)
        {
            int randMember = Random.Range(0, tmpList.Count);
            Visitor visitor = new Visitor(tmpList[randMember]);
            tmpList.RemoveAt(randMember);

            tempMembers.Add(visitor);

        }
        return tempMembers; ;
    }

    private List<T> GetRandomNumberOfElements<T>(List<T> list, int maxNumber)
    {
        List<T> tmpList = new List<T>();
        List<T> tmp = list.ToList();
        if (maxNumber > tmp.Count)
        {
            maxNumber = tmp.Count;
        }

        for (int i = 0; i < maxNumber; i++)
        {
            int randMember = Random.Range(0, tmp.Count);
            tmpList.Add(tmp[randMember]);
            tmp.RemoveAt(randMember);
        }

        return tmpList;
    }

    private void ShuffleList<T>(List<T> list)
    {
        var count = list.Count;
        int last = count - 1;

        for (int i = 0; i < last; i++)
        {
            var rand = Random.Range(0, last);
            var tmp = list[i];
            list[i] = list[rand];
            list[rand] = tmp;
        }
    }

    private Visitor SelectRandomVisitor()
    {
        int randIndex = Random.Range(0, visitorsList.Count);
        var randomVisitor = visitorsList[randIndex];
        visitorsList.RemoveAt(randIndex);

        return randomVisitor;
    }

    public void SetNextVisior()
    {
        currentVisitor = null;

        Visitor visitor = SelectRandomVisitor();
    }

    public void SendVisitorToRoom(KeyEnum draggedRoomKey)
    {
        foreach (var room in rooms)
        {
            if (room.roomKey == draggedRoomKey)
            {
                room.SetVisitor(currentVisitor);
            }
        }
    }
}
