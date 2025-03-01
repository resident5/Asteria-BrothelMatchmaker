using CHARACTERS;
using COMMANDS;
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

        /// <summary>
        /// Starts a conversation based on the lines from a text file or string list
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueManager.StartCoroutine(RunningConversation(conversation));

            return process;
        }
        
        /// <summary>
        /// Ends Coroutine prematurely if needed to start a nother
        /// </summary>
        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueManager.StopCoroutine(process);

            process = null;
        }

        /// <summary>
        /// Coroutine to run the actual conversation
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        IEnumerator RunningConversation(List<string> conversation)
        {
            dialogueManager.ActivateDialogueContainer(true);

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

                if (line.hasDialogue)
                    yield return WaitForUserInput();
            }

            dialogueManager.ActivateDialogueContainer(false);
        }

        /// <summary>
        /// Runs the dialogue section of the line if it exists then types it
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IEnumerator RunDialogue(DialogueLine line)
        {
            //Show/Hide the speakers name
            if (line.hasSpeaker)
            {
                HandleSpeakerLogic(line.speakerData);
            }
            yield return TypeLineSegments(line.dialogueData);
        }

        /// <summary>
        /// Handles the logic for the speaker section of the line
        /// </summary>
        /// <param name="speakerData"></param>
        private void HandleSpeakerLogic(SpeakerData speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition
                || speakerData.isCastingExpression);
            Character character = CharacterManager.Instance.GetCharacter(speakerData.name, createCharacter: false);

            //if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
            //{
            //    character.Show();
            //}
            if (speakerData.makeCharacterEnter)
            {
                if (character == null)
                    CharacterManager.Instance.CreateCharacter(speakerData.name, showAfterCreation: true);
                else
                    character.Show();
            }

            //Add character to UI
            dialogueManager.ShowSpeakerName(speakerData.displayName);

            dialogueManager.ApplySpeakerData(speakerData.displayName);

            //Cast position
            //if (speakerData.isCastingPosition)
            //    character.MoveToPosition(speakerData.castPosition);

            //Cast expressions
            //if(speakerData.isCastingExpression)
            //{
            //    foreach (var expression in speakerData.CastExpressions)
            //    {
            //        character.OnReceiveCastExpressions();
            //    }
            //}
        }

        /// <summary>
        /// Runs all the commands found in the line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IEnumerator RunCommands(DialogueLine line)
        {
            List<CommandData.Command> commandDatas = line.commandData.commands;

            foreach (var command in commandDatas)
            {
                if (command.waitForCompletion)
                    yield return CommandManager.instance.Execute(command.name, command.arguments);
                else
                    CommandManager.instance.Execute(command.name, command.arguments);
            }
            yield return null;
        }

        /// <summary>
        /// Types out the dialogue segments and waits for user input if needed based on signals
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IEnumerator TypeLineSegments(DialogueData line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DialogueData.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToTrigger(segment);

                yield return TypeDialogue(segment.dialogue, segment.appendText);
            }
        }

        /// <summary>
        /// Checks for dialogue signal in dialogue line then either waits for using input or a delay
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
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