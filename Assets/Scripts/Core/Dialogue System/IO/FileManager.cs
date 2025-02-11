using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;


public class FileManager
{
    public static List<string> ReadTextFiles(string filePath, bool includeBlankLines = true)
    {
        if (!filePath.StartsWith('/'))
        {
            filePath = FilePaths.root + filePath;
        }
        List<string> lines = new List<string>();
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (includeBlankLines || !string.IsNullOrEmpty(line))
                    {
                        lines.Add(line);
                    }
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError($"File not found: {e.FileName}");
        }

        return lines;
    }

    public static List<string> ReadTextAsset(string filePath, bool includeBlankLInes = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: {filePath}");
            return null;
        }


        return ReadTextAsset(asset, includeBlankLInes);
    }

    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLInes = false)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (includeBlankLInes || !string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }
        }

        return lines;
    }
}
