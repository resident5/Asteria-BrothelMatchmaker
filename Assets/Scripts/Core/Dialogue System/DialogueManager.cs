using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using CHARACTERS;

namespace DIALOGUE
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private DialogueSystemConfigurationSO config;
        public DialogueSystemConfigurationSO Config => config;

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

        public void ApplySpeakerData(string speakerName)
        {
            Character character = CharacterManager.Instance.GetCharacter(speakerName);
            CharacterConfigData config = character != null ? character.config : CharacterManager.Instance.GetCharacterConfig(speakerName);

            ApplySpeakerData(config);
        }

        public void ApplySpeakerData(CharacterConfigData config)
        {
            dialogueContainer.SetDialogueColor(config.dialogueColor);
            dialogueContainer.SetDialogueFont(config.dialogueFont);

            dialogueContainer.nameContainer.SetNameColor(config.nameColor);
            dialogueContainer.nameContainer.SetNameFont(config.nameFont);
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

        public void ActivateDialogueContainer(bool active)
        {
            dialogueContainer.ActivateDialogueBox(active);
        }

        public Coroutine Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
            return conversationManager.StartConversation(conversation);
        }
    }
}
