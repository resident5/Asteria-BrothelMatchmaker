using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.Plastic.Newtonsoft.Json.Linq;

public class JsonDesigner : EditorWindow
{

    public string defaultPath;
    public static JsonDesigner jsonDesigner;

    JsonData jsonEventData = new JsonData();
    Dictionary<string, string> narrationDict = new Dictionary<string, string>();

    public JObject jsonObject;

    public bool createDiagEvent;

    string jsonName;
    string json;
    string dialogueText = "";
    string nameText = "";
    string imageText = "";
    Vector2 scrollPosition;

    Rect rect = new Rect(10, 10, 200, 100);
    Rect tabbing = new Rect();
    Rect outerRect;
    Rect innerRect;
    //[SerializeField] TreeViewState treeViewState;
    //SimpleTreeView simpleTreeView;


    [MenuItem("Json/Json Designer")]
    public static void StartWindow()
    {
        jsonDesigner = EditorWindow.GetWindow<JsonDesigner>();
        jsonDesigner.minSize = new Vector2(250, 250);
        jsonDesigner.Show();
    }

    public void OnGUI()
    {
        //simpleTreeView.OnGUI(new Rect(0, 0, position.width, position.height));
        Create();
    }

    private void OnEnable()
    {
        defaultPath = Application.streamingAssetsPath + "/placeholder.json";
        tabbing = new Rect(20, 20, position.width - 40, position.height - 40);

        //if (treeViewState == null)
        //{
        //    treeViewState = new TreeViewState();
        //}
        //simpleTreeView = new SimpleTreeView(treeViewState);

        narrationDict.Add("narration", "");
        narrationDict.Add("name", "");
        narrationDict.Add("image", "");

        LoadJson();
    }

    public void Create()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        jsonName = EditorGUILayout.TextField(jsonName);

        ShowEditableJson();
        CreateTextFields();

        EditorGUILayout.BeginHorizontal();
        CreateDialogueEventButton();
        CreateNarrationEventButton();
        CreateDisplayEventButton();
        CreateEndTurnButton();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField(Application.streamingAssetsPath + "/" + jsonName + ".json");

        if (GUILayout.Button("Save"))
        {
            CreateTextFile();
        }
        //DrawRect();

        EditorGUILayout.EndScrollView();

    }

    void DrawRect()
    {
        float indentation = 20f;

        outerRect = GUILayoutUtility.GetRect(200, 150);
        outerRect.x += indentation;
        Handles.DrawSolidRectangleWithOutline(outerRect, new Color(.2f,.2f,.2f), Color.black);

        innerRect = GUILayoutUtility.GetRect(100, 50);
        innerRect.x += indentation * 1.5f;
        Handles.DrawSolidRectangleWithOutline(innerRect, Color.red, Color.black);
    }

    void CreateTextFile()
    {
        SaveJson();
        AssetDatabase.Refresh();

    }

    void CreateTextFields()
    {
        //JsonData jsonevent = new JsonData();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Diaglogue");
        dialogueText = EditorGUILayout.TextField(dialogueText);
        //jsonEventData.dialogueParam = EditorGUILayout.TextField(jsonEventData.dialogueParam);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Name");
        nameText = EditorGUILayout.TextField(nameText);
        //jsonEventData.nameParam = EditorGUILayout.TextField(jsonEventData.nameParam);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Image");
        imageText = EditorGUILayout.TextField(imageText);
        //jsonEventData.imageParam = EditorGUILayout.TextField(jsonEventData.imageParam);

        EditorGUILayout.EndHorizontal();

        Repaint();
    }

    void CreateDialogueEventButton()
    {
        if (GUILayout.Button("Diag"))
        {
            createDiagEvent = true;
        }
    }

    void CreateDisplayEventButton()
    {
        if (GUILayout.Button("Display"))
        {
        }

    }

    void CreateNarrationEventButton()
    {
        GUI.SetNextControlName("1");
        if (GUILayout.Button("Narration"))
        {
            dialogueText = EditorGUILayout.TextField(dialogueText);
            JObject tempObj = new JObject();
            tempObj["narration"] = dialogueText;
            tempObj["name"] = nameText;
            tempObj["image"] = imageText;
            //((JArray)jsonObject["diag"]).Add("Narration: " + dialogueText);
            //((JArray)jsonObject["diag"]).Add("name: " + nameText);
            //((JArray)jsonObject["diag"]).Add("image: " + imageText);
            ((JArray)jsonObject["diag"]).Add(tempObj);
            dialogueText = "";
            GUI.FocusControl("0");

            Repaint();
        }


    }

    void CreateEndTurnButton()
    {
        if (GUILayout.Button("End"))
        {

        }
    }

    void ShowEditableJson()
    {
        EditorGUILayout.TextField(jsonObject.ToString(), GUILayout.Height(250));

    }

    /// <summary>
    /// Load the Json if it exists... if it doesn't make a new Object and Array in the Json
    /// </summary>
    public void LoadJson()
    {
        if (!File.Exists(defaultPath))
        {
            //Create a Json Object and Create a diag array
            jsonObject = new JObject();
            jsonObject["diag"] = new JArray();
        }
        else
        {
            //Load the file at the path + parse the json
            string json = File.ReadAllText(defaultPath);
            jsonObject = JObject.Parse(json);
        }
    }

    /// <summary>
    /// Write to the Json file
    /// </summary>
    public void SaveJson()
    {
        File.WriteAllText(defaultPath, jsonObject.ToString());
    }

    void ContextMenu()
    {

    }


}

//public class SimpleTreeView : TreeView
//{
//    public SimpleTreeView(TreeViewState treeViewState) : base(treeViewState)
//    {
//        Reload();
//    }

//    protected override TreeViewItem BuildRoot()
//    {
//        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

//        var allItems = new List<TreeViewItem>
//        {
//            new TreeViewItem{id = 1, depth = 0, displayName = "Diag"},
//            new TreeViewItem{id = 2, depth = 1, displayName = "Narration"},
//            new TreeViewItem{id = 3, depth = 2, displayName = "Fire"},
//            new TreeViewItem{id = 4, depth = 2, displayName = "Bacon"},
//            new TreeViewItem{id = 5, depth = 1, displayName = "Savopr"},
//            new TreeViewItem{id = 6, depth = 2, displayName = "asdsad"}
//        };

//        SetupParentsAndChildrenFromDepths(root, allItems);

//        return root;
//    }
//}

//[Serializable]

//public class MyTreeElement : TreeElement
//{
//    public float floatVal;
//    public string text;
//    public bool enabled = true;

//    public MyTreeElement(string name, int depth, int id) : base(name,depth,id)
//    {
//        floatVal = 20f;
//    }

//}

//public class TreeElement
//{
//    [SerializeField] int id;
//    [SerializeField] string name;
//    [SerializeField] int depth;
//    [SerializeField] TreeElement parent;
//    [SerializeField] List<TreeElement> children;

//    public int Depth
//    {
//        get { return depth; }
//        set { depth = value; }
//    }

//    public TreeElement Parent
//    {
//        get { return parent; }
//        set { parent = value; }
//    }

//    public List<TreeElement> Children
//    {
//        get { return children; }
//        set { children = value; }
//    }

//    public bool hasChildren
//    {
//        get { return children != null && children.Count > 0; }
//    }

//    public string Name
//    {
//        get { return name; }
//        set { name = value; }
//    }

//    public int Id
//    {
//        get { return id; }
//        set { id = value; }
//    }

//    public TreeElement()
//    {

//    }

//    public TreeElement(string name, int depth, int id)
//    {
//        this.name = name;
//        this.depth = depth;
//        this.id = id;
//    }
//}

