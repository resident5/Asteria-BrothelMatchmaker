using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueManager dialogueManager => DialogueManager.Instance;

        Coroutine process = null;
        public bool isRunning = false;

        public TextArchitect architect;

        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueManager.onUserPromptNext += OnUserPromptNext;
        }

        private void OnUserPromptNext()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueManager.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueManager.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                //Show Dialogue
                DialogueLine line = DialogueParser.Parse(conversation[i]);

                if (line.hasDialogue)
                    yield return RunDialogue(line);


                if (line.hasCommands)
                    yield return RunCommands(line);
            }
        }

        IEnumerator RunDialogue(DialogueLine line)
        {
            //Show/Hide the speakers name
            if (line.hasSpeaker)
                dialogueManager.ShowSpeakerName(line.speaker.displayName);


            yield return TypeLineSegments(line.dialogue);

            //Wait

            yield return WaitForUserInput();

        }

        IEnumerator RunCommands(DialogueLine line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator TypeLineSegments(DialogueData line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DialogueData.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToTrigger(segment);

                yield return TypeDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToTrigger(DialogueData.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.C:
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.WC:
                case DialogueData.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator TypeDialogue(string dialogue, bool append = false)
        {
            if (!append)
                architect.Type(dialogue);
            else
                architect.AppendType(dialogue);

            while (architect.isTyping)
            {
                if (userPrompt)
                {
                    if (!architect.textSkip)
                        architect.textSkip = true;
                    else
                        architect.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
            {
                yield return null;
            }

            userPrompt = false;
        }
    }
}