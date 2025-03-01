﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace DIALOGUE
{
    public class CommandData
    {
        public List<Command> commands;
        private const char COMMANDSPLITTER_ID = ',';
        private const char ARGUEMENTCONTAINER_ID = '(';
        private const string WAITPREFIX_ID = "[wait]";


        public struct Command
        {
            public string name;
            public string[] arguments;
            public bool waitForCompletion;
        }

        public CommandData(string rawCommands)
        {
            commands = RipCommands(rawCommands);
        }

        public List<Command> RipCommands(string rawCommands)
        {
            string[] data = rawCommands.Split(COMMANDSPLITTER_ID, System.StringSplitOptions.RemoveEmptyEntries);
            List<Command> result = new List<Command>();

            foreach (var cmd in data)
            {
                Command command = new Command();
                int index = cmd.IndexOf(ARGUEMENTCONTAINER_ID);
                command.name = cmd.Substring(0, index).Trim();

                if (command.name.ToLower().StartsWith(WAITPREFIX_ID))
                {
                    command.name = command.name.Substring(WAITPREFIX_ID.Length);
                    command.waitForCompletion = true;
                }
                else
                {
                    command.waitForCompletion = false;
                }

                command.arguments = GetArgs(cmd.Substring(index + 1, cmd.Length - index - 2));
                result.Add(command);
            }

            return result;
        }

        private string[] GetArgs(string args)
        {
            List<string> argList = new List<string>();
            StringBuilder currentArg = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == '"')
                {
                    inQuotes = true;
                    continue;
                }

                if (!inQuotes && args[i] == ' ')
                {
                    argList.Add(currentArg.ToString());
                    currentArg.Clear();
                    continue;
                }

                currentArg.Append(args[i]);
            }

            if (currentArg.Length > 0)
            {
                argList.Add(currentArg.ToString());
            }

            return argList.ToArray();
        }
    }
}