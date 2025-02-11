using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

namespace DIALOGUE
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        public DialogueContainer dialogueContainer = new DialogueContainer();
        public ConversationManager conversationManager;
        private TextArchitect architect;

        public bool isRunningConversation => conversationManager.isRunning;

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPromptNext;

        public static DialogueManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Init();
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public void OnUserPromptNext()
        {
            onUserPromptNext?.Invoke();
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.nameContainer.Show(speakerName);
            else
                dialogueContainer.nameContainer.Hide();
        }
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        bool initialized = false;
        private void Init()
        {
            if (initialized)
                return;

            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);
        }

        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }
    }
}
