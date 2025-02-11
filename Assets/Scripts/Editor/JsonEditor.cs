using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JsonEditor : EditorWindow
{

    [MenuItem("Json/Json Editor")]
    public static void StartWindow()
    {
        var window = EditorWindow.GetWindow<JsonEditor>();
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    // Start is called before the first frame update
    //private void Create()
    //{
    //    text = EditorGUILayout.TextField(text);


    //    EditorGUILayout.BeginHorizontal();
    //    CreateDialogueEventButton();
    //    CreateNarrationEventButton();
    //    CreateDisplayEventButton();
    //    CreateEndTurnButton();
    //    EditorGUILayout.EndHorizontal();


    //    if (createDiagEvent == true)
    //    {
    //        CreateTextFields();
    //    }

    //}
}
