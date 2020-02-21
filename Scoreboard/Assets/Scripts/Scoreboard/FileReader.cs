using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public class FileReader
{
    public static Dictionary<string, string[]> LoadFileToDictionaryArray(string path)
    {
        var fileDict = new Dictionary<string, string[]>();
        TextAsset textAsset = (TextAsset) Resources.Load(path);
        foreach (var text in textAsset.text.Split('\n'))
        {
            var array = text.Replace("\"", "").Split('\t');
            fileDict.Add(array[0], new[] {array[1], array[2].Replace("\r", "")});
        }

        return fileDict;
    }
    public static Dictionary<string, string> LoadFileToDictionary(string path)
    {
        var fileDict = new Dictionary<string, string>();
        TextAsset textAsset = (TextAsset) Resources.Load(path);
        foreach (var text in textAsset.text.Split('\n'))
        {
            var array = text.Replace("\"", "").Split('\t');
            fileDict.Add(array[0], array[1].Replace("\r", ""));
        }

        return fileDict;
    }

    public static string[,] LoadURL(string url)
    {
        WebClient client = new WebClient();
        string downloadedString = client
            .DownloadString(
                url);

        var match = Regex.Match(downloadedString, @"(?<=<tbody.*>).+(?=</tbody>)", RegexOptions.Singleline);
        downloadedString = Regex.Replace(match.Value, "<[^>]*>", "");
        String[] data = downloadedString.Split('\t');

        var mMap = new Dictionary<string, string[]>();
        String team = "";
        int dataCounter = 0;
        int values = 0;
        foreach (var VARIABLE in data)
        {
            if (VARIABLE != "")
            {
                if (mMap.Keys.Count < 16 && Regex.IsMatch(VARIABLE, "[A-Za-z]") && !mMap.ContainsKey(VARIABLE))
                {
                    if (dataCounter == 18 || team == "")
                    {
                        mMap.Add(VARIABLE, new String[8]);
                        team = VARIABLE;
                        dataCounter = 0;
                        values = 0;
                    }
                    else if (team != "")
                    {
                        mMap[team][7] = VARIABLE.Replace("\r\n", "");
                    }
                }
                else
                {
                    dataCounter++;
                    if ((dataCounter == 2 || dataCounter == 3 || dataCounter == 4 || dataCounter == 5 ||
                         dataCounter == 6 || dataCounter == 15 || dataCounter == 16 || dataCounter == 17) && team != "")
                    {
                        if (mMap[team][values] == null)
                        {
                            mMap[team][values] = VARIABLE.Replace("\r\n", "");
                            values++;
                        }
                    }
                }
            }
        }

        string[,] toReturn = new string[mMap.Count, mMap[team].Length + 1];
        int x = 0;
        foreach (var key in mMap.Keys)
        {
            toReturn[x, 0] = key;
            int y = 1;
            foreach (var value in mMap[key])
            {
                toReturn[x, y] = value;
                y++;
            }

            x++;
        }

        return toReturn;
    }
}