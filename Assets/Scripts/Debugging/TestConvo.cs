using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestConvo : MonoBehaviour
{

    [SerializeField]
    private TextAsset file = null;

    private void Start()
    {
        StartConversation();
    }

    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset(file, false);

        //TestExpression(lines);
        DialogueManager.Instance.Say(lines);
    }

    void Test(List<string> lines)
    {
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
                return;

            Debug.Log($"Segmenting lines {line}");
            DialogueLine dLine = DialogueParser.Parse(line);
            int i = 0;

            foreach (DialogueData.DIALOGUE_SEGMENT segment in dLine.dialogue.segments)
            {
                Debug.Log($"Segment [{i++}] = '{segment.dialogue}' [signal = {segment.startSignal.ToString()}{(segment.signalDelay > 0 ? $"{segment.signalDelay}" : $"")}]");
            }
        }
    }

    void TestExpression(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            DialogueLine dl = DialogueParser.Parse(line);
            
            if (string.IsNullOrEmpty(line))
                continue;

            Debug.Log($"{dl.speaker.name} as [{(dl.speaker.castName != string.Empty ? dl.speaker.castName : dl.speaker.name)}] at " +
                $"{dl.speaker.castPosition}");

            List<(int l, string ex)> expr = dl.speaker.CastExpressions;
            for (int j = 0; j < expr.Count; j++)
            {
                Debug.Log($"[Layer[{expr[j].l} = '{expr[j].ex}']");
            }
        }
    }
}
