using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public TextAsset file;
    public string label;
    public string scriptName;

    public void Speak()
    {
        NaniNovelManager.instance.PlayDialogue(scriptName, label);
        //DialogueManager.Instance.Say(lines);
    }

    public void RemoveVisitor()
    {
        GameManager.Instance.RemoveVisitor();
    }
}
