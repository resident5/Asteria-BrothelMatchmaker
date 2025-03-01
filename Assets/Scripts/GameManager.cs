using Naninovel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public HUDController hudController;
    public enum GameState
    {
        START, //Start of the game (Handle intro dialogue, tutorial, etc)
        PLAYING, //Current Game Loop where the player will be for most
        SCORING, //Scoring part at the end of the playing loop when theres no more visitors
        END //End screen where all of the sex scenes are added
    }

    public GameState gameState = GameState.START;

    public Animator visitorAnimator;


    public int MAX_VISITORS = 6;
    public int MAX_MEMBERS = 2;

    //List of members that are supposed to be in
    //These are 100% accurate members that are pulled directly from their SOs

    [Header("For Debugging ONLY")]
    //All members for the current game
    [SerializeField] private List<Visitor> membersList;

    /// <summary>
    /// All rooms in the game
    /// </summary>
    [SerializeField] private Room[] rooms;

    /// <summary>
    /// All characters in the resources List
    /// </summary>
    [SerializeField] private List<VisitorSO> allCharacters;

    [SerializeField] private List<Visitor> visitorsList;
    Visitor currentVisitor;

    //Populate visitor list with all the members and then create copy cat members that are fake with some fake info in them

    public const string VISITOR_ENTER_ANIM = "Entering";
    public const string VISITOR_EXIT_ANIM = "Exiting";
    private const float nextVisitorDelay = 2f;

    public GameObject scoreBoard;

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
            Destroy(gameObject);
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
        if (Engine.Initialized)
            InitializeGame();
        else
            Engine.OnInitializationFinished += InitializeGame;

    }

    private void Update()
    {
        if (gameState == GameState.SCORING)
        {
            Debug.Log("SCORING TIME");
        }
    }

    private void OnDestroy()
    {
        Debug.Log("DONT DESTROY ME???");
    }

    public void InitializeGame()
    {
        allCharacters = Resources.LoadAll<VisitorSO>("Visitors").ToList();
        membersList = CreateMembersList();
        visitorsList = GetRandomNumberOfElements(membersList, MAX_MEMBERS);

        for (int i = 0; i < Mathf.Abs(MAX_VISITORS - MAX_MEMBERS); i++)
        {
            visitorsList.Add(CreateImpostersFromMembers());
        }

        ShuffleList(visitorsList);
        gameState = GameState.PLAYING;
        SelectVisitor();
    }

    private Visitor CreateImpostersFromMembers()
    {
        //Randomly choose an imposter type except for the first one cause that's None
        ImposterType impType = (ImposterType)Random.Range(1, System.Enum.GetValues(typeof(ImposterType)).Length);
        VisitorSO randomVisitorSO = allCharacters[Random.Range(0, allCharacters.Count)];
        Visitor visitor = new Visitor(randomVisitorSO);
        Visitor imposter = TurnVisitorIntoImposter(visitor, impType);

        imposter.imposterType = impType;
        imposter.isMember = false;

        return imposter;
    }

    private Visitor TurnVisitorIntoImposter(Visitor visitor, ImposterType imposters)
    {
        Visitor temp = visitor;

        switch (imposters)
        {
            case ImposterType.NONE:
                Debug.LogError("ERROR THIS IS FOR IMPOSTERS YOU SHOULD NOT BE NONE");
                break;
            case ImposterType.FAKE_NAME:
                temp.lastName = temp.lastName.Replace("e", "a");
                break;
            case ImposterType.FAKE_ID:
                temp.id = RandomDigit(temp.id);
                break;
            case ImposterType.FAKE_ICON:
                temp.id = RandomDigit(temp.id);
                break;
            case ImposterType.FAKE_EXPIRATION_DATE:
                temp.id = RandomDigit(temp.id);
                break;
        }

        return temp;
    }

    private string RandomDigit(string id)
    {
        string newID = "";
        //Debug.Log($"ID = {id}");
        int randomIndex = Random.Range(0, id.Length);
        char currentDigit = id[randomIndex];

        string newDigit = "" + currentDigit;
        while (currentDigit == newDigit[0])
        {
            int newNumber = Random.Range(0, 10);
            newDigit = "" + newNumber;
        }

        newID = id.Substring(0, randomIndex) + newDigit + id.Substring(randomIndex + 1);
        //Debug.Log($"New ID = {newID}");

        return newID;
    }

    public void SelectVisitor()
    {
        Coroutine enterProcess = StartCoroutine(VisitorEntering());

        if (enterProcess == null)
        {
            Debug.LogError("Enter Process is null");
        }
    }

    public void SetNextVisitor()
    {
        Coroutine exitProcess = StartCoroutine(VisitorExiting());

        if (exitProcess == null)
        {
            Debug.LogError("Exit Process is null");
        }
    }

    public IEnumerator VisitorEntering()
    {
        currentVisitor = SelectRandomVisitor();

        NaniNovelManager.instance.SetVariable(currentVisitor);

        hudController.SetupVisitor(currentVisitor);

        visitorAnimator.Play(VISITOR_ENTER_ANIM);

        yield return null;

        hudController.tableContainer.ShowId();
        hudController.tableContainer.ShowMemberID();
    }

    public IEnumerator VisitorExiting()
    {
        hudController.tableContainer.HideID();
        hudController.tableContainer.HideMemberID();

        visitorAnimator.Play(VISITOR_EXIT_ANIM);

        yield return new WaitForSeconds(nextVisitorDelay);

        if (visitorsList.Count > 0)
        {
            SelectVisitor();
        }
        else
        {
            gameState = GameState.SCORING;
            scoreBoard.SetActive(true);
            //Scoring();
        }


    }

    public IEnumerator VisitorRemoving()
    {
        hudController.tableContainer.HideID();
        hudController.tableContainer.HideMemberID();

        visitorAnimator.Play("Removing");

        yield return new WaitForSeconds(nextVisitorDelay);

        if (visitorsList.Count > 0)
        {
            SelectVisitor();
        }
        else
        {
            gameState = GameState.SCORING;
            scoreBoard.SetActive(true);
            //Scoring();
        }
    }

    public void RemoveVisitor()
    {
        StartCoroutine(VisitorRemoving());
    }

    public Room[] GetRooms() => rooms;

    public Visitor[] GetMembers() => membersList.ToArray();

    private IEnumerator Scoring()
    {
        foreach (var room in rooms)
        {
            room.GetFinalRoomScore();
        }
        yield return null;
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
        //Gets a random number of elements from list a max number of times and makes sure theres no duplicate
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
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }

        //var count = list.Count;
        //int last = count - 1;

        //for (int i = 0; i < last; i++)
        //{
        //    var rand = Random.Range(0, last);
        //    var tmp = list[i];
        //    list[i] = list[rand];
        //    list[rand] = tmp;
        //}
    }

    private Visitor SelectRandomVisitor()
    {
        int randIndex = Random.Range(0, visitorsList.Count);
        var randomVisitor = visitorsList[randIndex];
        visitorsList.RemoveAt(randIndex);

        return randomVisitor;
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
