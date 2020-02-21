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

    private Team homeTeam, awayTeam;
    private ScoreboardGUI scoreboardGui;

    public MatchController(ScoreboardGUI scoreboardGui, Team homeTeam, Team awayTeam)
    {
        this.scoreboardGui = scoreboardGui;
        this.homeTeam = homeTeam;
        this.awayTeam = awayTeam;
        startHalf = true;
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
        else if (Input.GetKeyDown(KeyCode.M))
            //INTRO
            scoreboardGui.StartCoroutine(scoreboardGui.PreMatch());
    }

    // TIME CONTROLLER
    private void StartTime()
    {
        if (playing)
        {
            if (MatchConfig.GetInstance().StoppedTime || EndFirstHalf() || EndSecondHalf())
                StopTime();
        }
        else if (!timeout)
        {
            if (startHalf)
            {
                time = MatchConfig.GetInstance().StoppedTime ? MatchConfig.GetInstance().MaxTime * 60 :
                    !firstHalf ? 0 : MatchConfig.GetInstance().MaxTime * 60;
                playing = true;
                startHalf = false;
                homeTeam.StartMatch();
                awayTeam.StartMatch();
                ChangeHalf();
            }
            else if (MatchConfig.GetInstance().StoppedTime)
            {
                playing = true;
            }

            scoreboardGui.StartCoroutine(
                scoreboardGui.StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
        }
    }

    private void UpdateTime()
    {
        if (MatchConfig.GetInstance().StoppedTime)
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
            finalString = "00.00";
            if (MatchConfig.GetInstance().StoppedTime)
                EndHalf();
        }

        scoreboardGui.Time.text = finalString;

        if (MatchConfig.GetInstance().StoppedTime && time == 0)
        {
        }
    }

    private void StopTime()
    {
        playing = false;

        if (!MatchConfig.GetInstance().StoppedTime)
        {
            if (firstHalf)
            {
                time = MatchConfig.GetInstance().MaxTime;
                startHalf = true;
                scoreboardGui.StartCoroutine(scoreboardGui.HalfMatch());
            }
            else if (secondHalf)
            {
                time = MatchConfig.GetInstance().MaxTime * 2;
                startHalf = true;
                scoreboardGui.StartCoroutine(scoreboardGui.EndMatch());
            }

            scoreboardGui.Time.text = time + ":00";

            scoreboardGui.StartCoroutine(
                scoreboardGui.StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
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

            scoreboardGui.StartCoroutine(
                scoreboardGui.StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
        }
    }

    // HALF CONTROLLER
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

    private void EndHalf()
    {
        playing = false;
        startHalf = true;
        if (firstHalf)
            scoreboardGui.StartCoroutine(scoreboardGui.HalfMatch());

        else if (secondHalf)
            scoreboardGui.StartCoroutine(scoreboardGui.EndMatch());


        scoreboardGui.StartCoroutine(scoreboardGui.StopUpperOrStartBottom(false));
    }

    private bool EndFirstHalf()
    {
        return playing && time / 60 >= MatchConfig.GetInstance().MaxTime - 1 && firstHalf;
    }

    private bool EndSecondHalf()
    {
        return playing && time / 60 >= MatchConfig.GetInstance().MaxTime * 2 - 1 && secondHalf;
    }

    // TIMEOUT CONTROLLER
    private void StartTimeout()
    {
        timeout = true;
        playing = false;
        scoreboardGui.StartCoroutine(scoreboardGui.Timeout());
    }

    public void StopTimeout()
    {
        timeout = false;
        if (!MatchConfig.GetInstance().StoppedTime)
            StartTime();
    }

    // FAULTS CONTROLLER
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

    private void ResetFaults()
    {
        homeTeam.ResetFaults();
        awayTeam.ResetFaults();
    }

    //GOALS CONTROLLER
    private void IncreaseScoreHome()
    {
        scoreboardGui.StopAllCoroutines();
        homeTeam.IncreaseScore(awayTeam);
        scoreboardGui.HomeScore.text = homeTeam.PlayingScore.ToString();
        if (MatchConfig.GetInstance().StoppedTime)
            StopTime();

        scoreboardGui.StartCoroutine(scoreboardGui.Goal("Home Goal"));
    }

    private void IncreaseScoreAway()
    {
        scoreboardGui.StopAllCoroutines();
        awayTeam.IncreaseScore(homeTeam);
        scoreboardGui.AwayScore.text = awayTeam.PlayingScore.ToString();
        // goal = true;
        if (MatchConfig.GetInstance().StoppedTime)
            StopTime();

        scoreboardGui.StartCoroutine(scoreboardGui.Goal("Away Goal"));
    }

    private void DecreaseScoreHome()
    {
        homeTeam.DecreaseScore(awayTeam);
        scoreboardGui.HomeScore.text = homeTeam.PlayingScore.ToString();
    }

    private void DecreaseScoreAway()
    {
        awayTeam.DecreaseScore(homeTeam);
        scoreboardGui.AwayScore.text = awayTeam.PlayingScore.ToString();
    }

    // GETTERS
    public bool Playing => playing;

    public bool Timeout => timeout;
}