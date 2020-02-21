using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchConfig
{
    private static MatchConfig mMatchConfig;
    private string mMatch;
    private bool mStoppedTime;
    private int mMaxTime;
    private string _mTableUrl;
    private bool updatedDay;
    
    private Dictionary<string,string[]> matchDict;

    private MatchConfig()
    {
        matchDict = FileReader.LoadFileToDictionaryArray("season");
    }

    public static MatchConfig GetInstance()
    {
        if(mMatchConfig==null)
            mMatchConfig=new MatchConfig();
        return mMatchConfig;
    }

    public Dictionary<string, string[]> MatchDict => matchDict;

    public int MaxTime
    {
        get => mMaxTime;
        set => mMaxTime = value;
    }

    public bool StoppedTime
    {
        get => mStoppedTime;
        set => mStoppedTime = value;
    }
    public bool UpdatedDay
    {
        get => updatedDay;
        set => updatedDay = value;
    }

    public string Match
    {
        get => mMatch;
        set => mMatch = value;
    }
    public string TableURL
    {
        get => _mTableUrl;
        set => _mTableUrl = value;
    }
}
