using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using COMMANDS;

namespace TESTING
{
    public class CMD_DatabaseExamples : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            //Add command with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print1p", new Action<string>(PrintUserMessage));
            database.AddCommand("printmp", new Action<string[]>(PrintLines));

            //Add lambda with no parameters
            database.AddCommand("printlambda", new Action(() => Debug.Log("Printing lambda message to console")));
            database.AddCommand("print1plambda", new Action<string>((args) => { Debug.Log($"Lambda Message {args}"); }));
            database.AddCommand("printmplambda", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            //Add coroutine with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("processmp", new Func<string[], IEnumerator>(MultiLineProcess));

            database.AddCommand("moveChar", new Func<string, IEnumerator>(MoveCharacter));
        }

        private static void PrintDefaultMessage()
        {
            //Debug.Log("Printing default message to console");
        }

        private static void PrintUserMessage(string message)
        {
            //Debug.Log($"User Message {message}");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (var item in lines)
            {
                Debug.Log($"Line {i++} = {item}");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 0; i <= 5; i++)
            {
                Debug.Log($"Process Running ... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 0; i <= num; i++)
                {
                    Debug.Log($"Process Running ... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }
        }

        private static IEnumerator MultiLineProcess(string[] data)
        {
            foreach (var line in data)
            {
                Debug.Log($"Process Running ... [{line}]");
                yield return new WaitForSeconds(0.5f);
            }
        }

        private static IEnumerator MoveCharacter(string data)
        {
            bool left = data.ToLower() == "left";

            Transform character = GameObject.Find("Visitor Image").transform;
            float moveSpeed = 15f;

            float targetX = left ? -8 : 8;

            float currentX = character.position.x;

            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
        }
    }
}