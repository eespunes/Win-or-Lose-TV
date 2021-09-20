using System;
using System.Collections.Generic;
using System.Linq;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class Loader
{
    public static Dictionary<string, string> LoadVideos()
    {
        var dict = new Dictionary<string, string>();
        dict.Add("Default", Application.dataPath + "/Videos/General/Default.mp4");
        dict.Add("Intro", Application.dataPath + "/Videos/General/Intro.mp4");
        dict.Add("Outro", Application.dataPath + "/Videos/General/Outro.mp4");
        dict.Add("Bottom",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_bottom.mp4");
        dict.Add("Upper Intro",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_upper_intro.mp4");
        dict.Add("Upper Loop",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_upper_loop.mp4");
        dict.Add("Upper Outro",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_upper_outro.mp4");
        dict.Add("Pre Match",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_pre.mp4");
        dict.Add("End Match",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_end.mp4");
        dict.Add("Half Time",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_half.mp4");
        dict.Add("Home Goal",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_goal_home.webm");
        dict.Add("Away Goal",
            Application.dataPath + "/Videos/Season/" + MatchConfig.GetInstance().Match + "_goal_away.webm");
        dict.Add("Timeout", Application.dataPath + "/Videos/General/Timeout.webm");
        dict.Add("Home Team", Application.dataPath + "/Videos/General/Home Team.webm");
        dict.Add("Table", Application.dataPath + "/Videos/Table/Table.mp4");

        return dict;
    }

    public static Dictionary<string, string> LoadSponsorVideos()
    {
        var dict = new Dictionary<string, string>();
        dict.Add("Sponsor Big", Application.dataPath + "/Videos/Sponsor/Sponsor_Big.mp4");
        dict.Add("Bottom",
            Application.dataPath + "/Videos/Sponsor/bottom_sponsor.webm");
        dict.Add("Upper Intro",
            Application.dataPath + "/Videos/Sponsor/upper_intro_sponsor.webm");
        dict.Add("Upper Loop",
            Application.dataPath + "/Videos/Sponsor/upper_loop_sponsor.webm");
        dict.Add("Upper Outro",
            Application.dataPath + "/Videos/Sponsor/upper_outro_sponsor.webm");
        dict.Add("Pre Match",
            Application.dataPath + "/Videos/Sponsor/pre_sponsor.webm");
        dict.Add("End Match",
            Application.dataPath + "/Videos/Sponsor/end_sponsor.webm");
        dict.Add("Half Time",
            Application.dataPath + "/Videos/Sponsor/half_sponsor.webm");
        dict.Add("Home Goal",
            Application.dataPath + "/Videos/Sponsor/goal_home_sponsor.webm");
        dict.Add("Away Goal",
            Application.dataPath + "/Videos/Sponsor/goal_away_sponsor.webm");
        dict.Add("Timeout", Application.dataPath + "/Videos/Sponsor/Timeout_sponsor.webm");
        dict.Add("Table", Application.dataPath + "/Videos/Sponsor/Table_sponsor.webm");

        return dict;
    }

    public static Dictionary<string, string[]> LoadTableVideos()
    {
        var dict = new Dictionary<string, string[]>();

        var lPlaying =
            FileBrowserHelpers.GetEntriesInDirectory(Application.dataPath + "/Videos/Table/Playing");
        var playing = new String[lPlaying.Count()];
        int lCounter = 0;
        foreach (var VARIABLE in lPlaying)
        {
            if (!VARIABLE.Path.Contains(".meta"))
            {
                playing[lCounter] = VARIABLE.Path;
                lCounter++;
            }
        }

        dict.Add("Playing", playing);

        var lNotPlaying =
            FileBrowserHelpers.GetEntriesInDirectory(Application.dataPath + "/Videos/Table/Not Playing");
        var notPlaying = new String[lNotPlaying.Count()];
        lCounter = 0;
        foreach (var VARIABLE in lNotPlaying)
        {
            if (!VARIABLE.Path.Contains(".meta"))
            {
                notPlaying[lCounter] = VARIABLE.Path;
                lCounter++;
            }
        }

        dict.Add("Not Playing", notPlaying);

        // var lRatxa = FileBrowserHelpers.GetEntriesInDirectory(Application.dataPath + "/Videos/Table/Streak");
        // var streak = new String[lPlaying.Count()];
        // lCounter = 0;
        // foreach (var VARIABLE in lRatxa)
        // {
        //     if (!VARIABLE.Path.Contains(".meta"))
        //     {
        //         streak[lCounter] = VARIABLE.Path;
        //         lCounter++;
        //     }
        // }
        //
        // dict.Add("Streak", streak);

        return dict;
    }

    public static Sprite LoadSpriteFromPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (FileBrowserHelpers.FileExists(path))
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            return sprite;
        }

        return null;
    }
}