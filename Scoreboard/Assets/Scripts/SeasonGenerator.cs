using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SeasonGenerator : MonoBehaviour
{
    public Dropdown mDropdown;
    public int mMatches;
    public bool mStoppedTIme;
    public int mMaxTimeStopped, mMaxTimeRunned;
    public String mGroupURL;

    private void Start()
    {
        SingletonMatchType.GetInstance().StoppedTime = mStoppedTIme;
        SingletonMatchType.GetInstance().MaxTime = mStoppedTIme ? mMaxTimeStopped : mMaxTimeRunned;
        SingletonMatchType.GetInstance().GroupURL = mGroupURL;
    }

    public void Generate()
    {
        mDropdown.ClearOptions();
        List<string> lSeason = new List<string>();
        for (int x = 1; x <= mMatches; x++)
        {
            string lName = "J";
            if (x > 9)
                lName += x.ToString();
            else
                lName += "0" + x.ToString();
            lSeason.Add(lName);
        }

        mDropdown.AddOptions(lSeason);
    }

    public void Reset()
    {
        mDropdown.ClearOptions();
    }

    public void ChangeScene()
    {
        SingletonMatchType.GetInstance().Match = mDropdown.options[mDropdown.value].text;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Match");
    }
}