using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser
    {
        // \\w Word character
        // \\w* Word character w/ wild card to find any character any space
        // \\w*[^\\s] Word of any length as long as its not a whitespace
        // \\( a choice
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";

        public static DialogueLine Parse(string rawLine)
        {
            //Debug.Log($"Parsing Line {rawLine}");

            (string speaker, string dialogue, string commands) = RipContent(rawLine);

            //Debug.Log($"Speaker = {speaker}\nDialogue = {dialogue}\nCommand = {commands}");

            return new DialogueLine(speaker, dialogue, commands);
        }

        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";
            int dialogueStart = -1;
            int dialogueEnd = -1;

            bool isEscaped = false;

            for (int i = 0; i < rawLine.Length; i++) //Loop through line
            {
                char currentChar = rawLine[i];
                if (currentChar == '\\') //Check if the first line is a escape 
                {
                    isEscaped = !isEscaped;
                }
                else if (currentChar == '"' && !isEscaped) //Is this the start or end of a quote? that's not a escape
                {
                    if (dialogueStart == -1) //Its the beginning of the quote right 
                    {
                        dialogueStart = i;
                    }
                    else if (dialogueEnd == -1) //Its the end of the quote
                    {
                        dialogueEnd = i;
                        break;
                    }
                }
                else
                {
                    isEscaped = false;
                }
            }

            //Debug.Log(rawLine.Substring(dialogueStart + 1, (dialogueEnd - dialogueStart) - 1));

            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);
            int commandStart = -1;
            
            foreach (Match match in matches)
            {
                //if the match is outside of the dialogue then we have a command
                if (match.Index < dialogueStart || match.Index > dialogueEnd)
                {
                    commandStart = match.Index;
                    break;
                }
            }

            //If we found a command but no dialogue
            if (commandStart != -1 && (dialogueStart == -1 && dialogueEnd == -1))
                return ("", "", rawLine.TrimEnd());

            //If we found dialogue already then we must be at the start of a command AFTER finding the dialogue
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //Get the command
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, (dialogueEnd - dialogueStart) - 1).Replace("\\\"", "\""); //Replacing all escape quotes 1with proper quotes
                if (commandStart != -1)
                {
                    commands = rawLine.Substring(commandStart).Trim();
                }
            } //If you found the commands but the dialogue is an argument
            else if (commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawLine;
            }
            else
            {
                dialogue = rawLine;
            }


            return (speaker, dialogue, commands);
        }
    }
}