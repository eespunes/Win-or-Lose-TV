using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeasonGenerator : MonoBehaviour
{
    [SerializeField] private Dropdown mDropdown;

    private string localTeam;

    private Dictionary<string, string> configDict;

    private void Start()
    {
        configDict = FileReader.LoadFileToDictionary("config");
        
        MatchConfig.GetInstance().StoppedTime = bool.Parse(configDict["Stopped Time"]);
        MatchConfig.GetInstance().MaxTime = MatchConfig.GetInstance().StoppedTime
            ? int.Parse(configDict["Maximum Stopped Time"])
            : int.Parse(configDict["Maximum Time"]);
        
        MatchConfig.GetInstance().TableURL = configDict["Table URL"];
    }

    public void Generate()
    {
        localTeam = FileReader.LoadFileToDictionary("config")["Local Team"];
        mDropdown.ClearOptions();
        List<string> lSeason = new List<string>();
        foreach (var match in MatchConfig.GetInstance().MatchDict.Keys)
        {
            string lName = match;
            int nameCounter = 0;
            foreach (var team in MatchConfig.GetInstance().MatchDict[match])
            {
                if (!localTeam.Equals(team))
                {
                    lName += "-" + team;
                }

                nameCounter++;
            }

            lSeason.Add(lName);
        }

        mDropdown.AddOptions(lSeason);
    }

    public void Reset()
    {
        mDropdown.ClearOptions();
    }


    // Updates once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            string thing = mDropdown.options[mDropdown.value].text;
            MatchConfig.GetInstance().Match = thing.Split('-')[0];
            SceneManager.LoadScene(1);
        }
    }
}