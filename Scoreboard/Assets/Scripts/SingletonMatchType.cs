using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMatchType
{
    private static SingletonMatchType mMatchType;
    private string mMatch;
    private bool mStoppedTime;
    private int mMaxTime;
    private string mGroupURL;

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

    public string Match
    {
        get => mMatch;
        set => mMatch = value;
    }

    private SingletonMatchType()
    {
    }

    public static SingletonMatchType GetInstance()
    {
        if(mMatchType==null)
            mMatchType=new SingletonMatchType();
        return mMatchType;
    }

    public string GroupURL
    {
        get => mGroupURL;
        set => mGroupURL = value;
    }
}
