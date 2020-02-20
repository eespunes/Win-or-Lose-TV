using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class Loader
{
    public static Dictionary<string, AudioClip> LoadAudios()
    {
        var dict = new Dictionary<string, AudioClip>();
        dict.Add("Intro", Resources.Load<AudioClip>("Audio/Intro"));
        dict.Add("Loop", Resources.Load<AudioClip>("Audio/Loop"));
        dict.Add("Outro", Resources.Load<AudioClip>("Audio/Ending"));
        return dict;
    }

    public static Dictionary<string, VideoClip> LoadVideos()
    {
        var dict = new Dictionary<string, VideoClip>();
        dict.Add("Intro", Resources.Load<VideoClip>("Videos/Untouchables/Intro"));
        dict.Add("Default", Resources.Load<VideoClip>("Videos/Untouchables/Default"));
        dict.Add("Outro", Resources.Load<VideoClip>("Videos/Untouchables/Outro"));
        dict.Add("Bottom",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_bottom"));
        dict.Add("Upper Intro",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_upper_intro"));
        dict.Add("Upper Loop",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_upper_loop"));
        dict.Add("Upper Outro",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_upper_outro"));
        dict.Add("Pre Match",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_pre"));
        dict.Add("End Match",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_end"));
        dict.Add("Half Intro",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_half_intro"));
        dict.Add("Half Loop",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_half_loop"));
        dict.Add("Half Outro",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_half_outro"));
        dict.Add("Home Goal",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_goal_home"));
        dict.Add("Away Goal",
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_goal_away"));
        dict.Add("Timeout", Resources.Load<VideoClip>("Videos/Untouchables/Timeout"));
        dict.Add("Table",  Resources.Load<VideoClip>("Videos/Table/table"));
        return dict;
    }


    public static Dictionary<string,VideoClip[]> LoadTableVideos()
    {
        var dict = new Dictionary<string, VideoClip[]>();
        var lPlaying = Resources.LoadAll("Videos/Table/Playing", typeof(VideoClip)).Cast<VideoClip>();
        var playing = new VideoClip[lPlaying.Count()];
        int lCounter = 0;
        foreach (var VARIABLE in lPlaying)
        {
            playing[lCounter] = VARIABLE;
            lCounter++;
        }

        dict.Add("Playing", playing);

        var lNotPlaying = Resources.LoadAll("Videos/Table/Not Playing", typeof(VideoClip)).Cast<VideoClip>();
        var notPlaying = new VideoClip[lNotPlaying.Count()];
        lCounter = 0;
        foreach (var VARIABLE in lNotPlaying)
        {
            notPlaying[lCounter] = VARIABLE;
            lCounter++;
        }
        dict.Add("Not Playing", notPlaying);

        var lRatxa = Resources.LoadAll("Videos/Table/Streak", typeof(VideoClip)).Cast<VideoClip>();
        var streak = new VideoClip[lPlaying.Count()];
        lCounter = 0;
        foreach (var VARIABLE in lRatxa)
        {
            streak[lCounter] = VARIABLE;
            lCounter++;
        }
        dict.Add("Streak", streak);
        return dict;
    }

}