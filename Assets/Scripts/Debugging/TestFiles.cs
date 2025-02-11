using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiles : MonoBehaviour
{
    private string fileName = "testFile";

    private void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        List<string> lines = FileManager.ReadTextFiles(fileName, true);

        foreach (var line in lines)
        {
            Debug.Log(line);
        }
        yield return null;
    }
}
