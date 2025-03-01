using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CommandDatabase : MonoBehaviour
    {
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        public bool hasCommand(string commandName) => database.ContainsKey(commandName);

        public void AddCommand(string commandName, Delegate command)
        {
            if (!database.ContainsKey(commandName))
            {
                database.Add(commandName, command);
            }
            else
            {
                Debug.LogError($"Command already exists in database {commandName}");
            }
        }

        public Delegate GetCommand(string commandName)
        {
            if (!database.ContainsKey(commandName))
            {
                Debug.LogError($"Command does not exist in database {commandName}");
                return null;
            }

            return database[commandName];
        }

    }
}