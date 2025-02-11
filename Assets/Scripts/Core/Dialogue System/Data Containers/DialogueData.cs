using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class DialogueData
{
    public List<DIALOGUE_SEGMENT> segments;
    private const string segmentIdentifierPatern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

    public bool hasDialogue => segments.Count > 0;

    public DialogueData(string rawDialogue)
    {
        segments = RipSegments(rawDialogue);
    }

    private List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue)
    {
        List<DIALOGUE_SEGMENT> tempSegments = new List<DIALOGUE_SEGMENT>();
        MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPatern);
        int lastIndex = 0;


        //Look for the first segment 
        DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();
        segment.dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
        segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE;
        segment.signalDelay = 0;
        tempSegments.Add(segment);

        if (matches.Count == 0)
            return tempSegments;
        else
            lastIndex = matches[0].Index;

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            segment = new DIALOGUE_SEGMENT();
            
            //Get Start signal for segment
            string signalMatch = match.Value;//{A}
            signalMatch = signalMatch.Substring(1, match.Length - 2);
            string[] signalSplit = signalMatch.Split(' ');

            segment.startSignal = (DIALOGUE_SEGMENT.StartSignal)Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());

            //Get the signal Signal Delay
            if (signalSplit.Length > 1)
                float.TryParse(signalSplit[1], out segment.signalDelay);

            //Get Dialogue for segment
            int nextIndex = i + 1 < matches.Count ? matches[i+1].Index : rawDialogue.Length;
            segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
            lastIndex = nextIndex;

            tempSegments.Add(segment);
        
        }

        return tempSegments;
    }

    public struct DIALOGUE_SEGMENT
    {
        public string dialogue;
        public StartSignal startSignal;
        public float signalDelay;
        public bool appendText => (startSignal == StartSignal.A || startSignal == StartSignal.WA);

        public enum StartSignal
        {
            NONE, C, A, WA, WC
        }
    }

}
