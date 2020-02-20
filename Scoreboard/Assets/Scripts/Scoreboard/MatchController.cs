using System.Collections;
using UnityEngine;

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

    public MatchController(ScoreboardGUI scoreboardGui)
    {
        this.scoreboardGui = scoreboardGui;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartTime();
        if (playing)
        {
            UpdateTime();
            // //GOAL HOME
            // if (Input.GetKeyDown(KeyCode.Q))
            //     IncreaseScoreHome();
            // if (Input.GetKeyDown(KeyCode.A))
            //     DecreaseScoreHome();
            // //GOAL AWAY
            // if (Input.GetKeyDown(KeyCode.W))
            //     IncreaseScoreAway();
            // if (Input.GetKeyDown(KeyCode.S))
            //     DecreaseScoreAway();
            // //FAULT HOME
            // if (Input.GetKeyDown(KeyCode.Z))
            //     HomeFault();
            // //FAULT AWAY
            // if (Input.GetKeyDown(KeyCode.X))
            //     AwayFault();
            // //TIMEOUT
            // if (Input.GetKeyDown(KeyCode.T))
            //     StartTimeout();
        }
    }

    private void StartTime()
    {
        if (playing)
        {
            if (SingletonMatchType.GetInstance().StoppedTime || EndFirstHalf() || EndSecondHalf())
                StopTime();
        }
        else
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

            if (timeout)
            {
                timeout = false;
                playing = true;
                StopUpperOrStartBottom();
            }
            else
            {
                if (time == SingletonMatchType.GetInstance().MaxTime * 60 ||
                    !SingletonMatchType.GetInstance().StoppedTime)
                {
                    StopUpperOrStartBottom();
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
            
            StopUpperOrStartBottom();
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

            StopUpperOrStartBottom();
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

    private IEnumerator StopUpperOrStartBottom()
    {
        InvokeRepeating(nameof(DecreaseSong), .25f, .25f);
         if (_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_upper_loop"))
         {
             _mAnimator.SetBool(Upper, false);
             _mMasterVideoPlayer.isLooping = false;
             ChangeVideo(_mUpperOutroVideoClip);
             Invoke(nameof(StopUpper), (float) _mUpperOutroVideoClip.length);
         }
         else if (!(_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_bottom")))
         {
             _mAnimator.SetTrigger(Bottom);
             ChangeVideo(_mBottomVideoClip);
             Invoke(nameof(StopBottom), (float) _mBottomVideoClip.length);
         }
    }

    private bool EndFirstHalf()
    {
        return playing && time / 60 >= SingletonMatchType.GetInstance().MaxTime - 1 && firstHalf;
    }

    private bool EndSecondHalf()
    {
        return playing && time / 60 >= SingletonMatchType.GetInstance().MaxTime * 2 - 1 && secondHalf;
    }

    public bool Playing => playing;
}