using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Scoreboard;
using SimpleFileBrowser;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;

    private string _localTeam;
    [SerializeField] private VideoPlayer teamVP;

    private Dictionary<string, string> _configDict;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _localTeam = PlayerPrefs.GetString("Local Team");
        dropdown.ClearOptions();

        List<string> season = new List<string>();
        season.Add("Partidos");
        for (int i = 0; i < PlayerPrefs.GetInt("Season Length"); i++)
        {
            string matchDay = (i + 1) < 10 ? "J0" + (i + 1) : "J" + (i + 1);
            string home = PlayerPrefs.GetString(matchDay + " Home");
            string away = PlayerPrefs.GetString(matchDay + " Away");
            
            var rivalTeam = home.Equals(_localTeam) ? away : home;

            if (i < (PlayerPrefs.GetInt("Season Length")))
            {
                PlayerPrefs.SetString("FindTeam " + i, rivalTeam);
            }

            season.Add(matchDay + "-" + rivalTeam);
        }

        dropdown.AddOptions(season);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        MatchConfig.GetInstance().StoppedTime = PlayerPrefs.GetString("Stopped Time").Length > 0 &&
                                                bool.Parse(PlayerPrefs.GetString("Stopped Time"));
        MatchConfig.GetInstance().ShowTable = PlayerPrefs.GetString("Show Table").Length > 0 &&
                                              bool.Parse(PlayerPrefs.GetString("Show Table"));
        MatchConfig.GetInstance().MaxTime = PlayerPrefs.GetInt("Maximum Time");

        MatchConfig.GetInstance().TableURL = PlayerPrefs.GetString("Table URL");
        MatchConfig.GetInstance().ResultURL = PlayerPrefs.GetString("Results URL");
        MatchConfig.GetInstance().LastMatchDayPlayed =
            FileController.LoadLastMatchPlayed(MatchConfig.GetInstance().ResultURL);

        var homeTeamUrl = Loader.LoadVideos()["Home Team"];
        if (FileBrowserHelpers.FileExists(homeTeamUrl))
            teamVP.url = homeTeamUrl;
    }

    public void ToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void StartMatch()
    {
        string match = dropdown.options[dropdown.value].text;
        var s = match.Split('-');
        MatchConfig.GetInstance().Match = s[0];
        PlayerPrefs.SetString("Rival Team", s[1]);
        SceneManager.LoadScene("Loading");
    }

    public void Exit()
    {
        Application.Quit();
    }
}