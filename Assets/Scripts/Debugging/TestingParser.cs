using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestingParser : MonoBehaviour
{
    public TextAsset file;
    private void Start()
    {
        ParseFile();
    }

    void ParseFile()
    {
        List<string> lines = FileManager.ReadTextAsset("testFile", false);

        foreach (string line in lines)
        {
            DialogueLine dl = DialogueParser.Parse(line);

        }
    }
}
