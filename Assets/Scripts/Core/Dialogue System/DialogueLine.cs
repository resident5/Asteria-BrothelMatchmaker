using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueLine
    {
        public SpeakerData speaker;
        public DialogueData dialogue;
        public string commands;

        public bool hasSpeaker => speaker != null;
        public bool hasDialogue => dialogue.hasDialogue;
        public bool hasCommands => commands != string.Empty;
    
        public DialogueLine(string speaker, string dialogue, string commands)
        {
            this.speaker = (string.IsNullOrWhiteSpace(speaker) ? null : new SpeakerData(speaker));
            this.dialogue = new DialogueData(dialogue);
            this.commands = commands;
        }
    }
}