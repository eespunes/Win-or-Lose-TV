using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MatchController
{
    private bool playing;

    private float time;

    private bool startHalf;
    private bool firstHalf;
    private bool secondHalf;
    private bool timeout;
    private bool goal;

    private Team homeTeam, awayTeam;
    private ScoreboardGUI scoreboardGui;

    public MatchController(ScoreboardGUI scoreboardGui, Team homeTeam, Team awayTeam)
    {
        this.scoreboardGui = scoreboardGui;
        this.homeTeam = homeTeam;
        this.awayTeam = awayTeam;
        scoreboardGui.HomeScore.text = homeTeam.PlayingScore.ToString();
        scoreboardGui.AwayScore.text = awayTeam.PlayingScore.ToString();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartTime();
        if (playing)
        {
            UpdateTime();
            //GOAL HOME
            if (Input.GetKeyDown(KeyCode.Q))
                IncreaseScoreHome();
            if (Input.GetKeyDown(KeyCode.A))
                DecreaseScoreHome();
            //GOAL AWAY
            if (Input.GetKeyDown(KeyCode.W))
                IncreaseScoreAway();
            if (Input.GetKeyDown(KeyCode.S))
                DecreaseScoreAway();
            //FAULT HOME
            if (Input.GetKeyDown(KeyCode.Z))
                HomeFault();
            //FAULT AWAY
            if (Input.GetKeyDown(KeyCode.X))
                AwayFault();
            //TIMEOUT
            if (Input.GetKeyDown(KeyCode.T))
                StartTimeout();
        }
    }

    private void StartTimeout()
    {
        timeout = true;
        playing = false;
        scoreboardGui.StartCoroutine(scoreboardGui.Timeout());
    }

    private void StartTime()
    {
        if (playing)
        {
            if (SingletonMatchType.GetInstance().StoppedTime || EndFirstHalf() || EndSecondHalf())
                StopTime();
        }
        else if (!timeout)
        {
            if (startHalf)
            {
                time = SingletonMatchType.GetInstance().StoppedTime ? SingletonMatchType.GetInstance().MaxTime * 60 :
                    !firstHalf ? 0 : SingletonMatchType.GetInstance().MaxTime * 60;
                playing = true;
                startHalf = false;
                ChangeHalf();
            }
            else if (SingletonMatchType.GetInstance().StoppedTime)
            {
                playing = false;
                if (goal)
                    goal = false;
            }

            else
            {
                if (time == SingletonMatchType.GetInstance().MaxTime * 60 ||
                    !SingletonMatchType.GetInstance().StoppedTime)
                {
                    scoreboardGui.StartCoroutine(scoreboardGui.StopUpperOrStartBottom());
                }

                playing = true;
            }
        }
    }

    private void UpdateTime()
    {
        if (SingletonMatchType.GetInstance().StoppedTime)
            time -= 1 * Time.deltaTime;
        else
            time += 1 * Time.deltaTime;

        int lMinutesInt = (int) time / 60;
        string lAddMinutes = lMinutesInt < 10 ? "0" : "";

        float lSecondsFloat = time % 60;
        string lAddSeconds = lSecondsFloat < 10 ? "0" : "";


        var commaSplit = lSecondsFloat.ToString("f2").Split(',');

        string finalString = lMinutesInt == 0
            ? lAddSeconds +
              (commaSplit.Length != 2 ? lSecondsFloat.ToString("f2") : commaSplit[0] + "." + commaSplit[1])
            : lAddMinutes + lMinutesInt + ":" + lAddSeconds + ((int) lSecondsFloat).ToString("f0");

        if (lMinutesInt == 0 && lSecondsFloat <= 0)
        {
            time = 0;
            finalString = "00:00";
        }

        scoreboardGui.Time.text = finalString;

        if (SingletonMatchType.GetInstance().StoppedTime && time == 0)
        {
            StopTime();
        }
    }

    private void StopTime()
    {
        playing = false;

        if (!SingletonMatchType.GetInstance().StoppedTime)
        {
            if (firstHalf)
            {
                time = SingletonMatchType.GetInstance().MaxTime;
                startHalf = true;
            }
            else if (secondHalf)
            {
                time = SingletonMatchType.GetInstance().MaxTime * 2;
                startHalf = true;
            }

            scoreboardGui.Time.text = time + ":00";

            scoreboardGui.StartCoroutine(scoreboardGui.StopUpperOrStartBottom());
        }
        else if (time == 0)
        {
            if (firstHalf)
            {
                startHalf = true;
            }
            else if (secondHalf)
            {
                startHalf = true;
            }

            scoreboardGui.StartCoroutine(scoreboardGui.StopUpperOrStartBottom());
        }
    }

    private void ChangeHalf()
    {
        if (!firstHalf && !secondHalf)
        {
            firstHalf = true;
        }
        else if (firstHalf)
        {
            firstHalf = false;
            secondHalf = true;
        }

        ResetFaults();
    }

    private void ResetFaults()
    {
        // _mHomeFaults = 0;
        // _mAwayFaults = 0;
        //
        // foreach (var fault in _mHomeFaultsUI)
        // {
        //     fault.SetActive(false);
        // }
        //
        // foreach (var fault in _mAwayFaultsUI)
        // {
        //     fault.SetActive(false);
        // }
    }

    private bool EndFirstHalf()
    {
        return playing && time / 60 >= SingletonMatchType.GetInstance().MaxTime - 1 && firstHalf;
    }

    private bool EndSecondHalf()
    {
        return playing && time / 60 >= SingletonMatchType.GetInstance().MaxTime * 2 - 1 && secondHalf;
    }

    private void IncreaseScoreHome()
    {
        scoreboardGui.StopAllCoroutines();
        homeTeam.IncreaseScore(awayTeam);
        scoreboardGui.HomeScore.text = homeTeam.PlayingScore.ToString();
        // goal = true;
        if (SingletonMatchType.GetInstance().StoppedTime)
            StopTime();

        scoreboardGui.StartCoroutine(scoreboardGui.Goal("Home Goal"));
    }

    private void DecreaseScoreHome()
    {
        homeTeam.DecreaseScore(awayTeam);
        scoreboardGui.HomeScore.text = homeTeam.PlayingScore.ToString();
    }

    private void IncreaseScoreAway()
    {
        scoreboardGui.StopAllCoroutines();
        awayTeam.IncreaseScore(homeTeam);
        scoreboardGui.AwayScore.text = awayTeam.PlayingScore.ToString();
        // goal = true;
        if (SingletonMatchType.GetInstance().StoppedTime)
            StopTime();

        scoreboardGui.StartCoroutine(scoreboardGui.Goal("Away Goal"));
    }

    private void DecreaseScoreAway()
    {
        awayTeam.DecreaseScore(homeTeam);
        scoreboardGui.AwayScore.text = awayTeam.PlayingScore.ToString();
    }
    
    private void HomeFault()
    {
        homeTeam.IncreaseFault();
        scoreboardGui.StartCoroutine(scoreboardGui.HomeFaults(homeTeam.PlayingFaults));
    }


    private void AwayFault()
    {
        awayTeam.IncreaseFault();
        scoreboardGui.StartCoroutine(scoreboardGui.AwayFaults(awayTeam.PlayingFaults));
    }

    public bool Playing => playing;

    public bool Timeout => timeout;

    public void StopTimeout()
    {
        timeout = false;
        StartTime();
    }
}