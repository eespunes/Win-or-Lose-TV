using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scoreboard
{
    public class FileController
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

        public static string[,] LoadTableURL(string url)
        {
            WebClient client = new WebClient();
            string downloadedString = client
                .DownloadString(url);

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
                        if (dataCounter == 20 || team == "")
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
                        if ((dataCounter == 2 || dataCounter == 4 || dataCounter == 5 ||
                             dataCounter == 6 || dataCounter == 7 || dataCounter == 17 ||
                             dataCounter == 18||
                             dataCounter == 19) && team != "")
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

        public static int LoadLastMatchPlayed(string url)
        {
            for (int matchDay = 1; matchDay <= PlayerPrefs.GetInt("Season Length"); matchDay++)
            {
                WebClient client = new WebClient();
                string downloadedString = client
                    .DownloadString(
                        url + matchDay);

                var match = Regex.Match(downloadedString, @"(?<=<table.*>).+(?=</table>)", RegexOptions.Singleline);
                downloadedString = Regex.Replace(match.Value, "<[^>]*>", "");
                String[] data = downloadedString.Split('\t');
                bool local = false;
                bool breaked = false;
                foreach (string VARIABLE in data)
                {
                    if (VARIABLE != "")
                    {
                        if (TableGenerator.FindTeam(VARIABLE) >= 0) local = !local;
                        else if (local)
                        {
                            if (VARIABLE.Split('-').Length == 2)
                            {
                                breaked = true;
                                break;
                            }
                        }
                    }
                }

                if (!breaked)
                    return matchDay;
            }

            return -1;
        }

        public static int[] LoadResultURL(string url, string homeTeam)
        {
            WebClient client = new WebClient();
            string downloadedString = client.DownloadString(url);

            var match = Regex.Match(downloadedString, @"(?<=<table.*>).+(?=</table>)", RegexOptions.Singleline);
            downloadedString = Regex.Replace(match.Value, "<[^>]*>", "");
            String[] data = downloadedString.Split('\t');
            bool local = false;
            homeTeam = homeTeam.Replace("\n", "");
            foreach (string VARIABLE in data)
            {
                if (VARIABLE != "")
                {
                    if (TableGenerator.FindTeam(VARIABLE) >= 0 && VARIABLE.Equals(homeTeam))
                    {
                        local = !local;
                    }
                    else if (local)
                    {
                        var strings = VARIABLE.Split('-');
                        if (strings.Length == 2)
                        {
                            return new int[] {Int32.Parse(strings[0]), Int32.Parse(strings[1])};
                        }
                        else if (VARIABLE.Split(':').Length == 2)
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }
    }
}