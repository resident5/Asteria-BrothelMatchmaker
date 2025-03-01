using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager instance { get; private set; }
        private CommandDatabase commandDatabase;

        private static Coroutine process = null;
        public static bool isProcessRunning => process != null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                commandDatabase = new CommandDatabase();

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                foreach (var ext in extensionTypes)
                {
                    MethodInfo extendMethod = ext.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { commandDatabase });
                }
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        public Coroutine Execute(string commandName, params string[] args)
        {
            Delegate command = commandDatabase.GetCommand(commandName);

            if (command == null)
                return null;

            if (command is Action)
                command.DynamicInvoke();
            else if (command is Action<string>)
                command.DynamicInvoke(args[0]);
            else if (command is Action<string[]>)
                command.DynamicInvoke((object)args);

            return StartProcess(commandName, command, args);
        }

        public Coroutine StartProcess(string commandName, Delegate command, params string[] args)
        {
            StopCurrentProcess();

            process = StartCoroutine(RunningProcess(command, args));

            return process;
        }

        private void StopCurrentProcess()
        {
            if (process != null)
            {
                StopCoroutine(process);
            }
            process = null;
        }

        private IEnumerator RunningProcess(Delegate command, string[] args)
        {
            yield return WaitingForFinishedProcess(command, args);

            process = null;

        }

        private IEnumerator WaitingForFinishedProcess(Delegate command, string[] args)
        {
            if (command is Action)
                command.DynamicInvoke();
            else if (command is Action<string>)
                command.DynamicInvoke(args[0]);
            else if (command is Action<string[]>)
                command.DynamicInvoke((object)args);
            else if (command is Func<IEnumerator>)
                yield return ((Func<IEnumerator>)command)();
            else if (command is Func<string, IEnumerator>)
                yield return ((Func<string, IEnumerator>)command)(args[0]);
            else if (command is Func<string[], IEnumerator>)
                yield return ((Func<string[], IEnumerator>)command)(args);
        }
    }
}