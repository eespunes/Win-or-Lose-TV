using System.Collections;
using System.Collections.Generic;
using Scoreboard;
using UnityEngine;

public class MatchConfig
{
    private static MatchConfig _instance;

    public int MaxTime { get; set; }

    public bool StoppedTime { get; set; }

    public string Match { get; set; }
    
    public int LastMatchDayPlayed { get; set; }

    public string TableURL { get; set; }
    
    public string ResultURL { get; set; }
    public bool ShowTable { get; set; }

    private MatchConfig()
    {
    }

    public static MatchConfig GetInstance()
    {
        return _instance ?? (_instance = new MatchConfig());
    }
}