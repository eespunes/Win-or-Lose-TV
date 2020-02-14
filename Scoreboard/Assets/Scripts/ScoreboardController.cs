using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Text.RegularExpressions;

public class ScoreboardController : MonoBehaviour
{
    private VideoPlayer _mMasterVideoPlayer, _mTimeoutVideoPlayer;

    private AudioSource _mAudioSource;

    private Text _mTimeText, _mHomeScoreText, _mAwayScoreText;

    private float _mTime;
    private bool _mTimeout, _mStoppedTime;
    private int _mMaxTime;
    private int _mHomeScore, _mAwayScore;
    private int _mHomeFaults, _mAwayFaults;
    private bool _mPlaying;

    private GameObject[] _mHomeFaultsUI, _mAwayFaultsUI;

    private AudioClip _mIntroAudioClip, _mLoopAudioClip, _mGoalAudioClip, _mOutroAudioClip;

    private VideoClip _mIntroVideoClip,
        _mDefaultVideoClip,
        _mOutroVideoClip,
        _mBottomVideoClip,
        _mUpperIntroVideoClip,
        _mUpperLoopVideoClip,
        _mUpperOutroVideoClip,
        _mPreMatchVideoClip,
        _mEndMatchVideoClip,
        _mHalftimeIntroVideoClip,
        _mHalftimeLoopVideoClip,
        _mHalfTimeOutroVideoClip,
        _mHomeGoalVideoClip,
        _mAwayGoalVideoClip,
        _mConnectedVideoClip,
        _mDisconnectedVideoClip,
        _mTableVideoClip;

    private VideoClip[] mRatxa, mNotPlaying, mPlaying;

    private bool _mStartHalf;
    private bool _mFirstHalf;
    private bool _mSecondHalf;
    private bool _mGoal;

    private Animator _mAnimator;
    private static readonly int Bottom = Animator.StringToHash("Bottom");
    private static readonly int Upper = Animator.StringToHash("Upper");
    private static readonly int HalfTime = Animator.StringToHash("Half Time");
    private static readonly int EndTime = Animator.StringToHash("End Match");
    private static readonly int ToUpperIntro = Animator.StringToHash("To Upper Intro");
    private static readonly int ToIdle = Animator.StringToHash("To Idle");

    private Dictionary<string, string[]> mMap;
    private Text[] mPunts, mJugats, mGuanyats, mEmpatats, mPerduts;
    private VideoPlayer[] mEquips, mRatxa1, mRatxa2, mRatxa3, mRatxa4;
    public Transform mTableRenderer;

    public Color mNotPlayingColor, mPlayingColor;
    private Dictionary<string, string[]> mFileMap;
    private bool halfTablePlayed;

    private void Awake()
    {
        _mAudioSource = GetComponent<AudioSource>();
        _mAnimator = transform.GetChild(3).GetComponent<Animator>();
        _mStoppedTime = SingletonMatchType.GetInstance().StoppedTime;
        _mMaxTime = SingletonMatchType.GetInstance().MaxTime;
        FindVideoPlayer();
        FindTexts();
        FindTable();
        LoadAudio();
        LoadVideos();

        _mMasterVideoPlayer.clip = _mDefaultVideoClip;

        LoadClassification();
    }

    private void LoadFile()
    {
        mFileMap = new Dictionary<string, string[]>();
        TextAsset mytxtData = (TextAsset) Resources.Load("season");
        foreach (var text in mytxtData.text.Split('\n'))
        {
            var array = text.Replace("\"", "").Split('\t');
            mFileMap.Add(array[0], new[] {array[1], array[2].Replace("\r", "")});
        }
    }

    private void FillClassification()
    {
        int lCounter = 0;
        foreach (var lTeam in mMap.Keys)
        {
            int jCounter = 0;
            if (PlayingTeam(lTeam))
            {
                mEquips[lCounter].clip = mPlaying[FindTeam(lTeam)];
                foreach (var value in mMap[lTeam])
                {
                    string lValue = value.Replace("\n", "");

                    switch (jCounter)
                    {
                        case 0:
                            mPerduts[lCounter].text = lValue;
                            mPerduts[lCounter].color = mPlayingColor;
                            break;
                        case 1:
                            mPunts[lCounter].text = lValue;
                            mPunts[lCounter].color = mPlayingColor;

                            break;
                        case 2:
                            mEmpatats[lCounter].text = lValue;
                            mEmpatats[lCounter].color = mPlayingColor;
                            break;
                        case 3:
                            mGuanyats[lCounter].text = lValue;
                            mGuanyats[lCounter].color = mPlayingColor;
                            break;
                        case 4:
                            mJugats[lCounter].text = lValue;
                            mJugats[lCounter].color = mPlayingColor;
                            break;
                        case 5:
                        case 6:
                            break;
                        case 7:
                            int zCounter = 0;
                            foreach (var letter in lValue)
                            {
                                if (letter == 'G')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[3];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[3];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[3];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[3];
                                            break;
                                    }

                                    zCounter++;
                                }
                                else if (letter == 'E')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[1];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[1];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[1];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[1];
                                            break;
                                    }

                                    zCounter++;
                                }
                                else if (letter == 'P')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[0];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[0];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[0];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[0];
                                            break;
                                    }

                                    zCounter++;
                                }
                                else if (letter == 'S' || letter == 'N')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[2];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[2];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[2];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[2];
                                            break;
                                    }

                                    zCounter++;
                                }
                            }

                            break;
                    }

                    jCounter++;
                }
            }
            else
            {
                mEquips[lCounter].clip = mNotPlaying[FindTeam(lTeam)];
                foreach (var value in mMap[lTeam])
                {
                    string lValue = value.Replace("\n", "");

                    switch (jCounter)
                    {
                        case 0:
                            mPerduts[lCounter].text = lValue;
                            mPerduts[lCounter].color = mNotPlayingColor;
                            break;
                        case 1:
                            mPunts[lCounter].text = lValue;
                            mPunts[lCounter].color = mNotPlayingColor;
                            break;
                        case 2:
                            mEmpatats[lCounter].text = lValue;
                            mEmpatats[lCounter].color = mNotPlayingColor;
                            break;
                        case 3:
                            mGuanyats[lCounter].text = lValue;
                            mGuanyats[lCounter].color = mNotPlayingColor;
                            break;
                        case 4:
                            mJugats[lCounter].text = lValue;
                            mJugats[lCounter].color = mNotPlayingColor;
                            break;
                        case 5:
                        case 6:
                            break;
                        case 7:
                            int zCounter = 0;
                            foreach (var letter in value)
                            {
                                if (letter == 'G')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[3];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[3];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[3];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[3];
                                            break;
                                    }

                                    zCounter++;
                                }
                                else if (letter == 'E')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[1];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[1];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[1];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[1];
                                            break;
                                    }

                                    zCounter++;
                                }
                                else if (letter == 'P')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[0];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[0];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[0];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[0];
                                            break;
                                    }

                                    zCounter++;
                                }
                                else if (letter == 'S' || letter == 'N')
                                {
                                    switch (zCounter)
                                    {
                                        case 1:
                                            mRatxa1[lCounter].clip = mRatxa[2];
                                            break;
                                        case 2:
                                            mRatxa2[lCounter].clip = mRatxa[2];
                                            break;
                                        case 3:
                                            mRatxa3[lCounter].clip = mRatxa[2];
                                            break;
                                        case 4:
                                            mRatxa4[lCounter].clip = mRatxa[2];
                                            break;
                                    }

                                    zCounter++;
                                }
                            }

                            break;
                    }

                    jCounter++;
                }
            }

            lCounter++;
        }
    }

    private int FindTeam(string team)
    {
        team = team.Replace("\n", "");
        switch (team)
        {
            case "ACCIÓ SANT MARTI ASSOC CC,B":
                return 0;
            case "SANT ANDREU, A.E.,C":
                return 1;
            case "CARMELO-SUNLIFE F.S.,A":
                return 2;
            case "CASP, A.E.,A":
                return 3;
            case "CET10 (ESCOLA BAC DE RODA),C":
                return 4;
            case "LES GLORIES 2014 CLUB FUTBOL SALA,B":
                return 5;
            case "GRÀCIA FUTBOL SALA CLUB,B":
                return 6;
            case "HORTA, U.AT.,A":
                return 7;
            case "IPSE EL PILAR, C.E.,A":
                return 8;
            case "PLAZA MACAEL CCA,A":
                return 9;
            case "ATLETIC LA PALMA  ASSOCIACIO ESPORTIVA ,A":
                return 10;
            case "FUTSAL POLARIS FORT PIENC,B":
                return 11;
            case "PROSPERITAT NOU BARRIS CLUB ESP. FUTBOL SALA,B":
                return 12;
            case "THAU, C.E.,A":
                return 13;
            case "CLUB  ESPORTIU VINCIT-PROVENÇALENC,A":
                return 14;
            case "CENTRE MONTSERRAT XAVIER 1904,B":
                return 15;
            default:
                return 0;
        }
    }

    private bool PlayingTeam(string team)
    {
        team = team.Replace("\n", "");
        return team.Equals(mFileMap[SingletonMatchType.GetInstance().Match][0]) ||
               team.Equals(mFileMap[SingletonMatchType.GetInstance().Match][1]);
    }

    private void LoadClassification()
    {
        WebClient client = new WebClient();
        String downloadedString = client
            .DownloadString(
                SingletonMatchType.GetInstance().GroupURL);
        
        var match = Regex.Match(downloadedString, @"(?<=<tbody.*>).+(?=</tbody>)",RegexOptions.Singleline);
        downloadedString = Regex.Replace(match.Value, "<[^>]*>", "");
        print(downloadedString);
        String[] data = downloadedString.Split('\t');
        mMap = new Dictionary<string, string[]>();
        String key = "";
        int dataCounter = 0;
        int values = 0;
        foreach (var VARIABLE in data)
        {
            if (VARIABLE != "")
            {
                if (mMap.Keys.Count < 16 && Regex.IsMatch(VARIABLE, "[A-Za-z]") && !mMap.ContainsKey(VARIABLE))
                {
                    if (dataCounter == 18 || key == "")
                    {
                        mMap.Add(VARIABLE, new String[8]);
                        key = VARIABLE;
                        dataCounter = 0;
                        values = 0;
                    }
                    else if (key != "")
                    {
                        mMap[key][7] = VARIABLE.Replace("\r\n", "");
                    }
                }
                else
                {
                    dataCounter++;
                    if ((dataCounter == 2 || dataCounter == 3 || dataCounter == 4 || dataCounter == 5 ||
                         dataCounter == 6 || dataCounter == 15 || dataCounter == 16 || dataCounter == 17) && key != "")
                    {
                        if (mMap[key][values] == null)
                        {
                            mMap[key][values] = VARIABLE.Replace("\r\n", "");
                            values++;
                        }
                    }
                }
            }
        }

        LoadFile();
        CalculateTableBeforePlaying();
        FillClassification();
    }


    void Start()
    {
        _mStartHalf = true;

        _mAwayScoreText.text = _mAwayScore.ToString();
        _mHomeScoreText.text = _mHomeScore.ToString();
    }

    void Update()
    {
        //TIME
        if (Input.GetKeyDown(KeyCode.Space))
            StartTime();
        if (_mPlaying)
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
        else
        {
            //INTRO
            if (Input.GetKeyDown(KeyCode.M))
                Intro();
            // IntroTable();
        }
    }

    //        METHODS CREATED BY KEY
    private void Intro()
    {
        _mAudioSource.clip = _mIntroAudioClip;
        _mAudioSource.Play();
        ChangeVideo(_mIntroVideoClip);
        Invoke(nameof(StopIntro), (float) _mIntroVideoClip.length);
    }

    private void Outro()
    {
        _mAudioSource.clip = _mOutroAudioClip;
        ChangeVideo(_mOutroVideoClip);
        _mAudioSource.Play();
    }

    private void StartTime()
    {
        if (_mPlaying)
        {
            if (_mStoppedTime || EndFirstHalf() || EndSecondHalf())
                StopTime();
        }
        else
        {
            if (_mStartHalf)
            {
                _mTime = _mStoppedTime ? _mMaxTime * 60 : !_mFirstHalf ? 0 : _mMaxTime * 60;
                _mPlaying = true;
                _mStartHalf = false;
                ChangeHalf();
            }
            else if (_mStoppedTime)
            {
                _mPlaying = false;
                if (_mGoal)
                    _mGoal = false;
            }

            if (_mTimeout)
            {
                _mTimeout = false;
                _mPlaying = true;
                StopUpperOrStartBottom();
            }
            else
            {
                if (_mTime == _mMaxTime * 60 || !_mStoppedTime)
                {
                    StopUpperOrStartBottom();
                }

                _mPlaying = true;
            }
        }
    }

    private void UpdateTime()
    {
        if (_mStoppedTime)
            _mTime -= 1 * Time.deltaTime;
        else
            _mTime += 1 * Time.deltaTime;

        int lMinutesInt = (int) _mTime / 60;
        string lAddMinutes = lMinutesInt < 10 ? "0" : "";

        float lSecondsFloat = _mTime % 60;
        string lAddSeconds = lSecondsFloat < 10 ? "0" : "";


        var commaSplit = lSecondsFloat.ToString("f2").Split(',');

        string finalString = lMinutesInt == 0
            ? lAddSeconds +
              (commaSplit.Length != 2 ? lSecondsFloat.ToString("f2") : commaSplit[0] + "." + commaSplit[1])
            : lAddMinutes + lMinutesInt + ":" + lAddSeconds + ((int) lSecondsFloat).ToString("f0");

        if (lMinutesInt == 0 && lSecondsFloat <= 0)
        {
            _mTime = 0;
            finalString = "00:00";
        }

        _mTimeText.text = finalString;

        if (_mStoppedTime && _mTime == 0)
        {
            StopTime();
        }
    }

    private void IncreaseScoreHome()
    {
        CancelInvoke();
        _mAnimator.SetTrigger(ToIdle);
        _mHomeScore++;
        _mGoal = true;
        if (_mStoppedTime)
            StopTime();

        StopUpperOrStartBottom();

        Invoke(nameof(StartGoalHome), (float) _mUpperOutroVideoClip.length);
    }

    private void DecreaseScoreHome()
    {
        if (_mHomeScore > 0)
            _mHomeScore--;
        _mHomeScoreText.text = _mHomeScore.ToString();
    }

    private void IncreaseScoreAway()
    {
        CancelInvoke();
        _mAnimator.SetTrigger(ToIdle);
        _mAwayScore++;
        _mGoal = true;
        if (_mStoppedTime)
            StopTime();

        StopUpperOrStartBottom();

        Invoke(nameof(StartGoalAway), (float) _mUpperOutroVideoClip.length);
    }

    private void DecreaseScoreAway()
    {
        if (_mAwayScore > 0)
            _mAwayScore--;
        _mAwayScoreText.text = _mAwayScore.ToString();
    }

    private void HomeFault()
    {
        _mHomeFaults++;
        if (_mHomeFaults == 1)
            _mHomeFaultsUI[_mHomeFaults - 1].SetActive(true);
        else if (_mHomeFaults <= 5)
        {
            _mHomeFaultsUI[_mHomeFaults - 1].SetActive(true);
            _mHomeFaultsUI[_mHomeFaults - 2].SetActive(false);
        }
        else
            return;

        if (!(_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_bottom")))
        {
            if (_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_upper_loop"))
                _mMasterVideoPlayer.frame = 0;
            else
            {
                _mMasterVideoPlayer.isLooping = false;
                _mAnimator.SetTrigger(ToUpperIntro);
                ChangeVideo(_mUpperIntroVideoClip);
                Invoke(nameof(StartUpperLoop), (float) _mUpperIntroVideoClip.length);
            }
        }
    }


    private void AwayFault()
    {
        _mAwayFaults++;
        if (_mAwayFaults == 1)
            _mAwayFaultsUI[_mAwayFaults - 1].SetActive(true);
        else if (_mAwayFaults <= 5)
        {
            _mAwayFaultsUI[_mAwayFaults - 1].SetActive(true);
            _mAwayFaultsUI[_mAwayFaults - 2].SetActive(false);
        }
        else
            return;

        if (!(_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_bottom")))
        {
            if (_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_upper_loop"))
                _mMasterVideoPlayer.frame = 0;
            else
            {
                _mMasterVideoPlayer.isLooping = false;
                _mAnimator.SetTrigger(ToUpperIntro);
                ChangeVideo(_mUpperIntroVideoClip);
                Invoke(nameof(StartUpperLoop), (float) _mUpperIntroVideoClip.length);
            }
        }
    }

    private void StartTimeout()
    {
        _mTimeout = true;
        _mPlaying = false;
        if (!_mGoal)
        {
            _mTimeoutVideoPlayer.gameObject.SetActive(true);
            if (!(_mMasterVideoPlayer.isPlaying && _mMasterVideoPlayer.clip.name.Contains("_upper_loop")))
            {
                if (!_mGoal)
                {
                    _mAnimator.SetBool(Upper, true);
                    ChangeVideo(_mUpperIntroVideoClip);
                    Invoke(nameof(StartUpperLoop), (float) _mUpperIntroVideoClip.length);
                }
            }

            Invoke(nameof(StopTimeout), (float) _mTimeoutVideoPlayer.clip.length);
        }
    }

    private void StopTime()
    {
        _mPlaying = false;

        if (!_mStoppedTime)
        {
            if (_mFirstHalf)
            {
                _mTime = _mMaxTime;
                _mStartHalf = true;
            }
            else if (_mSecondHalf)
            {
                _mTime = _mMaxTime * 2;
                _mStartHalf = true;
            }

            _mTimeText.text = _mTime + ":00";

            StopUpperOrStartBottom();
        }
        else if (_mTime == 0)
        {
            if (_mFirstHalf)
            {
                _mStartHalf = true;
            }
            else if (_mSecondHalf)
            {
                _mStartHalf = true;
            }

            StopUpperOrStartBottom();
        }
    }

//        INVOKE METHODS    
    private void StopIntro()
    {
        _mAudioSource.clip = _mLoopAudioClip;
        _mAudioSource.loop = true;
        _mAudioSource.Play();
        StopVideo();
        Invoke(nameof(PreMatch), 5f);
    }

    private void PreMatch()
    {
        ChangeVideo(_mPreMatchVideoClip);
        Invoke(nameof(StopPreMatch), (float) _mPreMatchVideoClip.length);
    }

    private void StopPreMatch()
    {
        StopVideo();
        Invoke(nameof(IntroTable), 5f);
    }

    private void IntroTable()
    {
        ChangeVideo(_mTableVideoClip);
        mTableRenderer.gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        Invoke(nameof(StopVideo), (float) _mTableVideoClip.length);
    }

    private void HalfTable()
    {
        ChangeVideo(_mTableVideoClip);
        halfTablePlayed = true;
        mTableRenderer.gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        Invoke(nameof(StartHalfTime), (float) _mTableVideoClip.length - 1);
    }

    private void EndTable()
    {
        ChangeVideo(_mTableVideoClip);
        mTableRenderer.gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        Invoke(nameof(StopTable), (float) _mTableVideoClip.length);
    }

    private void StopTable()
    {
        StopVideo();
        Invoke(nameof(Outro), 5f);
    }

    private void StopUpperOrStartBottom()
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

    private void StopBottom()
    {
        if (_mPlaying || _mGoal || _mTimeout)
        {
            if (_mGoal)
                _mGoal = false;
            _mAnimator.SetBool(Upper, true);
            ChangeVideo(_mUpperIntroVideoClip);
            Invoke(nameof(StartUpperLoop), (float) _mUpperIntroVideoClip.length);
        }
        else
        {
            CancelInvoke();
            StopVideo();
            InvokeRepeating(nameof(IncreaseSong), .25f, .25f);
            _mAudioSource.Play();

            CalculateActualTable();
            Invoke(_mFirstHalf ? nameof(StartHalfTime) : nameof(StartEndTime), 5f);
        }
    }

    private void CalculateActualTable()
    {
        LoadClassification();
        String[,] lTable = new string[16, 9];

        //Crear Nueva Clasificación
        int lCounter = 0;
        int lHome = 0, lAway = 0;
        foreach (var team in mMap.Keys)
        {
            int jCounter = 0;
            lTable[lCounter, jCounter] = team;
            jCounter++;
            foreach (var value in mMap[team])
            {
                int lValue = 0;
                Int32.TryParse(value, out lValue);
                if (PlayingTeam(team))
                {
                    if (team.Replace("\n", "").Equals(mFileMap[SingletonMatchType.GetInstance().Match][0]))
                    {
                        lHome = lCounter;
                        if (jCounter == 1) //Puntos
                        {
                            if (_mHomeScore > _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 3).ToString();
                            else if (_mHomeScore == _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 2) //Jornadas
                        {
                            lTable[lCounter, jCounter] = (lValue + 1).ToString();
                        }
                        else if (jCounter == 3) //Victorias
                        {
                            if (_mHomeScore > _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 4) //Empates
                        {
                            if (_mHomeScore == _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 5) //Derrotas
                        {
                            if (_mHomeScore < _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 6) //Goles a Favor
                        {
                            lTable[lCounter, jCounter] = (lValue + _mHomeScore).ToString();
                        }
                        else if (jCounter == 7) //Goles en Contra
                        {
                            lTable[lCounter, jCounter] = (lValue + _mAwayScore).ToString();
                        }
                        else if (jCounter == 8) //Racha
                        {
                            string s2 = Regex.Replace(value, @"[^A-Z]+", String.Empty);
                            string s3 = s2.Substring(s2.Length - 1);
                            int lPunts, lVictories, lEmpats, lDerrotes;
                            Int32.TryParse(lTable[lCounter, 1], out lPunts);
                            Int32.TryParse(lTable[lCounter, 3], out lVictories);
                            Int32.TryParse(lTable[lCounter, 4], out lEmpats);
                            Int32.TryParse(lTable[lCounter, 5], out lDerrotes);
                            if (s3.Equals("G"))
                            {
                                lTable[lCounter, 1] = (lPunts - 3).ToString();
                                lTable[lCounter, 3] = (lVictories - 1).ToString();
                            }
                            else if (s3.Equals("P"))
                            {
                                lTable[lCounter, 5] = (lDerrotes - 1).ToString();
                            }
                            else if (s3.Equals("E"))
                            {
                                lTable[lCounter, 1] = (lPunts - 1).ToString();
                                lTable[lCounter, 4] = (lEmpats - 1).ToString();
                            }

                            if (_mHomeScore > _mAwayScore)
                                s2 = s2.Substring(0, s2.Length - 1) + "G";
                            else if (_mHomeScore == _mAwayScore)
                                s2 = s2.Substring(0, s2.Length - 1) + "E";
                            else
                                s2 = s2.Substring(0, s2.Length - 1) + "P";
                            lTable[lCounter, jCounter] = s2;
                        }
                    }
                    else
                    {
                        lAway = lCounter;
                        if (jCounter == 1) //Puntos
                        {
                            if (_mHomeScore < _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 3).ToString();
                            else if (_mHomeScore == _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 2) //Jornadas
                        {
                            lTable[lCounter, jCounter] = (lValue + 1).ToString();
                        }
                        else if (jCounter == 3) //Victorias
                        {
                            if (_mHomeScore < _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 4) //Empates
                        {
                            if (_mHomeScore == _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 5) //Derrotas
                        {
                            if (_mHomeScore > _mAwayScore)
                                lTable[lCounter, jCounter] = (lValue + 1).ToString();
                            else
                                lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 6) //Goles a Favor
                        {
                            lTable[lCounter, jCounter] = (lValue + _mAwayScore).ToString();
                        }
                        else if (jCounter == 7) //Goles en Contra
                        {
                            lTable[lCounter, jCounter] = (lValue + _mHomeScore).ToString();
                        }
                        else if (jCounter == 8) //Racha
                        {
                            string s2 = Regex.Replace(value, @"[^A-Z]+", String.Empty);
                            string s3 = s2.Substring(s2.Length - 1);
                            int lPunts, lVictories, lEmpats, lDerrotes;
                            Int32.TryParse(lTable[lCounter, 1], out lPunts);
                            Int32.TryParse(lTable[lCounter, 3], out lVictories);
                            Int32.TryParse(lTable[lCounter, 4], out lEmpats);
                            Int32.TryParse(lTable[lCounter, 5], out lDerrotes);
                            if (s3.Equals("G"))
                            {
                                lTable[lCounter, 1] = (lPunts - 3).ToString();
                                lTable[lCounter, 3] = (lVictories - 1).ToString();
                            }
                            else if (s3.Equals("P"))
                            {
                                lTable[lCounter, 5] = (lDerrotes - 1).ToString();
                            }
                            else if (s3.Equals("E"))
                            {
                                lTable[lCounter, 1] = (lPunts - 1).ToString();
                                lTable[lCounter, 4] = (lEmpats - 1).ToString();
                            }

                            if (_mHomeScore > _mAwayScore)
                                s2 = s2.Substring(0, s2.Length - 1) + "P";
                            else if (_mHomeScore == _mAwayScore)
                                s2 = s2.Substring(0, s2.Length - 1) + "E";
                            else
                                s2 = s2.Substring(0, s2.Length - 1) + "G";
                            lTable[lCounter, jCounter] = s2;
                        }
                    }
                }
                else
                {
                    if (jCounter == 8)
                        lTable[lCounter, jCounter] = value;
                    else
                        lTable[lCounter, jCounter] = lValue.ToString();
                }

                jCounter++;
            }

            lCounter++;
        }

        String[] t = new string[9];

        Debug.LogWarning(lHome);
        Debug.LogWarning(lAway);

        //Ordenar la clasificación
        for (int i = 0; i < 16; i++)
        {
            if (i == lHome)
                for (int j = lHome - 1; j >= 0; j--)
                {
                    var c1 = 0;
                    var c2 = 0;
                    var f1 = 0;
                    var f2 = 0;
                    var p1 = 0;
                    var p2 = 0;

                    Int32.TryParse(lTable[lHome, 7], out c1);
                    Int32.TryParse(lTable[j, 7], out c2);
                    Int32.TryParse(lTable[lHome, 6], out f1);
                    Int32.TryParse(lTable[j, 6], out f2);
                    Int32.TryParse(lTable[lHome, 1], out p1);
                    Int32.TryParse(lTable[j, 1], out p2);
                    // print(lTable[lHome, 0] + ", "+p1+ ", "+f1+ ", "+c1+"== " + lTable[j, 0]+ ", "+p2+ ", "+f2+ ", "+c2);
                    if ((p1 > p2) ||
                        (p1 == p2 && f1 - c1 > f2 - c2) ||
                        (f1 - c1 == f2 - c2 && f1 > f2) ||
                        (f1 == f2 && c1 > c2))
                    {
                        // print("\tENTERS");
                        for (int k = 0; k < 9; k++)
                            t[k] = lTable[lHome, k];
                        for (int k = 0; k < 9; k++)
                            lTable[lHome, k] = lTable[j, k];
                        for (int k = 0; k < 9; k++)
                            lTable[j, k] = t[k];
                        lHome = j;
                    }
                }
            else if (i == lAway)
            {
                for (int j = lAway - 1; j >= 0; j--)
                {
                    var c1 = 0;
                    var c2 = 0;
                    var f1 = 0;
                    var f2 = 0;
                    var p1 = 0;
                    var p2 = 0;

                    Int32.TryParse(lTable[lAway, 7], out c1);
                    Int32.TryParse(lTable[j, 7], out c2);
                    Int32.TryParse(lTable[lAway, 6], out f1);
                    Int32.TryParse(lTable[j, 6], out f2);
                    Int32.TryParse(lTable[lAway, 1], out p1);
                    Int32.TryParse(lTable[j, 1], out p2);
                    // print(lTable[lAway, 0] + ", "+p1+ ", "+f1+ ", "+c1+"== " + lTable[j, 0]+ ", "+p2+ ", "+f2+ ", "+c2);

                    if ((p1 > p2) ||
                        (p1 == p2 && f1 - c1 > f2 - c2) ||
                        (f1 - c1 == f2 - c2 && f1 > f2) ||
                        (f1 == f2 && c1 > c2))
                    {
                        // print("\tENTERS");
                        for (int k = 0; k < 9; k++)
                            t[k] = lTable[lAway, k];
                        for (int k = 0; k < 9; k++)
                            lTable[lAway, k] = lTable[j, k];
                        for (int k = 0; k < 9; k++)
                            lTable[j, k] = t[k];
                        lAway = j;
                    }
                }
            }
        }

        //Pasarlo a Dictionary   
        mMap = new Dictionary<string, string[]>();
        for (int i = 0; i < 16; i++)
        {
            mMap.Add(lTable[i, 0],
                new[]
                {
                    lTable[i, 1], lTable[i, 2], lTable[i, 3], lTable[i, 4], lTable[i, 5], lTable[i, 6], lTable[i, 7],
                    lTable[i, 8]
                });
        }

        FillClassification();
    }

    private void CalculateTableBeforePlaying()
    {
        String[,] lTable = new string[16, 9];

        //Crear Nueva Clasificación
        int lCounter = 0;
        int lHome = 0, lAway = 0;
        foreach (var team in mMap.Keys)
        {
            int jCounter = 0;
            lTable[lCounter, jCounter] = team;
            jCounter++;
            foreach (var value in mMap[team])
            {
                int lValue = 0;
                Int32.TryParse(value, out lValue);
                if (PlayingTeam(team))
                {
                    if (team.Replace("\n", "").Equals(mFileMap[SingletonMatchType.GetInstance().Match][0]))
                    {
                        lHome = lCounter;
                        if (jCounter == 1) //Puntos
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 2) //Jornadas
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 3) //Victorias
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 4) //Empates
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 5) //Derrotas
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 6) //Goles a Favor
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 7) //Goles en Contra
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 8) //Racha
                        {
                            if (SingletonMatchType.GetInstance().UpdatedDay)
                            {
                                string s2 = Regex.Replace(value, @"[^A-Z]+", String.Empty);
                                string s3 = s2.Substring(s2.Length - 2);
                                int lPunts, lVictories, lEmpats, lDerrotes, lJornades;
                                Int32.TryParse(lTable[lCounter, 1], out lPunts);
                                Int32.TryParse(lTable[lCounter, 2], out lJornades);
                                Int32.TryParse(lTable[lCounter, 3], out lVictories);
                                Int32.TryParse(lTable[lCounter, 4], out lEmpats);
                                Int32.TryParse(lTable[lCounter, 5], out lDerrotes);

                                if (s3[0] == 'G')
                                {
                                    lTable[lCounter, 1] = (lPunts - 3).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 3] = (lVictories - 1).ToString();
                                }
                                else if (s3[0] == 'P')
                                {
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 5] = (lDerrotes - 1).ToString();
                                }
                                else if (s3[0] == 'E')
                                {
                                    lTable[lCounter, 1] = (lPunts - 1).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 4] = (lEmpats - 1).ToString();
                                }

                                s2 = s2.Substring(0, s2.Length - 2) + "NN";
                                lTable[lCounter, jCounter] = s2;
                            }
                            else
                            {
                                string s2 = Regex.Replace(value, @"[^A-Z]+", String.Empty);
                                string s3 = s2.Substring(s2.Length - 1);
                                int lPunts, lVictories, lEmpats, lDerrotes, lJornades;
                                Int32.TryParse(lTable[lCounter, 1], out lPunts);
                                Int32.TryParse(lTable[lCounter, 2], out lJornades);
                                Int32.TryParse(lTable[lCounter, 3], out lVictories);
                                Int32.TryParse(lTable[lCounter, 4], out lEmpats);
                                Int32.TryParse(lTable[lCounter, 5], out lDerrotes);

                                if (s3.Equals("G"))
                                {
                                    lTable[lCounter, 1] = (lPunts - 3).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 3] = (lVictories - 1).ToString();
                                }
                                else if (s3.Equals("P"))
                                {
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 5] = (lDerrotes - 1).ToString();
                                }
                                else if (s3.Equals("E"))
                                {
                                    lTable[lCounter, 1] = (lPunts - 1).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 4] = (lEmpats - 1).ToString();
                                }

                                s2 = s2.Substring(0, s2.Length - 1) + "N";
                                lTable[lCounter, jCounter] = s2;
                            }
                        }
                    }
                    else
                    {
                        lAway = lCounter;
                        if (jCounter == 1) //Puntos
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 2) //Jornadas
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 3) //Victorias
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 4) //Empates
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 5) //Derrotas
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 6) //Goles a Favor
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 7) //Goles en Contra
                        {
                            lTable[lCounter, jCounter] = lValue.ToString();
                        }
                        else if (jCounter == 8) //Racha
                        {
                            if (SingletonMatchType.GetInstance().UpdatedDay)
                            {
                                string s2 = Regex.Replace(value, @"[^A-Z]+", String.Empty);
                                string s3 = s2.Substring(s2.Length - 2);
                                int lPunts, lVictories, lEmpats, lDerrotes, lJornades;
                                Int32.TryParse(lTable[lCounter, 1], out lPunts);
                                Int32.TryParse(lTable[lCounter, 2], out lJornades);
                                Int32.TryParse(lTable[lCounter, 3], out lVictories);
                                Int32.TryParse(lTable[lCounter, 4], out lEmpats);
                                Int32.TryParse(lTable[lCounter, 5], out lDerrotes);

                                if (s3[0] == 'G')
                                {
                                    lTable[lCounter, 1] = (lPunts - 3).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 3] = (lVictories - 1).ToString();
                                }
                                else if (s3[0] == 'P')
                                {
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 5] = (lDerrotes - 1).ToString();
                                }
                                else if (s3[0] == 'E')
                                {
                                    lTable[lCounter, 1] = (lPunts - 1).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 4] = (lEmpats - 1).ToString();
                                }

                                s2 = s2.Substring(0, s2.Length - 2) + "NN";
                                lTable[lCounter, jCounter] = s2;
                            }
                            else
                            {
                                string s2 = Regex.Replace(value, @"[^A-Z]+", String.Empty);
                                string s3 = s2.Substring(s2.Length - 1);
                                int lPunts, lVictories, lEmpats, lDerrotes, lJornades;
                                Int32.TryParse(lTable[lCounter, 1], out lPunts);
                                Int32.TryParse(lTable[lCounter, 2], out lJornades);
                                Int32.TryParse(lTable[lCounter, 3], out lVictories);
                                Int32.TryParse(lTable[lCounter, 4], out lEmpats);
                                Int32.TryParse(lTable[lCounter, 5], out lDerrotes);

                                if (s3.Equals("G"))
                                {
                                    lTable[lCounter, 1] = (lPunts - 3).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 3] = (lVictories - 1).ToString();
                                }
                                else if (s3.Equals("P"))
                                {
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 5] = (lDerrotes - 1).ToString();
                                }
                                else if (s3.Equals("E"))
                                {
                                    lTable[lCounter, 1] = (lPunts - 1).ToString();
                                    lTable[lCounter, 2] = (lJornades - 1).ToString();
                                    lTable[lCounter, 4] = (lEmpats - 1).ToString();
                                }

                                s2 = s2.Substring(0, s2.Length - 1) + "N";
                                lTable[lCounter, jCounter] = s2;
                            }
                        }
                    }
                }
                else
                {
                    if (jCounter == 8)
                        lTable[lCounter, jCounter] = value;
                    else
                        lTable[lCounter, jCounter] = lValue.ToString();
                }

                jCounter++;
            }

            lCounter++;
        }

        String[] t = new string[9];

        //Ordenar la clasificación
        for (int i = 0; i < 16; i++)
        {
            if (i == lHome)
                for (int j = lHome + 1; j < 16; j++)
                {
                    var c1 = 0;
                    var c2 = 0;
                    var f1 = 0;
                    var f2 = 0;
                    var p1 = 0;
                    var p2 = 0;

                    Int32.TryParse(lTable[lHome, 7], out c1);
                    Int32.TryParse(lTable[j, 7], out c2);
                    Int32.TryParse(lTable[lHome, 6], out f1);
                    Int32.TryParse(lTable[j, 6], out f2);
                    Int32.TryParse(lTable[lHome, 1], out p1);
                    Int32.TryParse(lTable[j, 1], out p2);

                    if ((p1 < p2) ||
                        (p1 == p2 && f1 - c1 < f2 - c2) ||
                        (f1 - c1 == f2 - c2 && f1 < f2) ||
                        (f1 == f2 && c1 < c2))
                    {
                        for (int k = 0; k < 9; k++)
                            t[k] = lTable[lHome, k];
                        for (int k = 0; k < 9; k++)
                            lTable[lHome, k] = lTable[j, k];
                        for (int k = 0; k < 9; k++)
                            lTable[j, k] = t[k];
                        lHome = j;
                    }
                }
            else if (i == lAway)
            {
                for (int j = lAway + 1; j < 16; j++)
                {
                    var c1 = 0;
                    var c2 = 0;
                    var f1 = 0;
                    var f2 = 0;
                    var p1 = 0;
                    var p2 = 0;

                    Int32.TryParse(lTable[lAway, 7], out c1);
                    Int32.TryParse(lTable[j, 7], out c2);
                    Int32.TryParse(lTable[lAway, 6], out f1);
                    Int32.TryParse(lTable[j, 6], out f2);
                    Int32.TryParse(lTable[lAway, 1], out p1);
                    Int32.TryParse(lTable[j, 1], out p2);

                    if ((p1 < p2) ||
                        (p1 == p2 && f1 - c1 < f2 - c2) ||
                        (f1 - c1 == f2 - c2 && f1 < f2) ||
                        (f1 == f2 && c1 < c2))
                    {
                        for (int k = 0; k < 9; k++)
                            t[k] = lTable[lAway, k];
                        for (int k = 0; k < 9; k++)
                            lTable[lAway, k] = lTable[j, k];
                        for (int k = 0; k < 9; k++)
                            lTable[j, k] = t[k];
                        lAway = j;
                    }
                }
            }
        }

        //Pasarlo a Dictionary   
        mMap = new Dictionary<string, string[]>();
        for (int i = 0; i < 16; i++)
        {
            mMap.Add(lTable[i, 0],
                new[]
                {
                    lTable[i, 1], lTable[i, 2], lTable[i, 3], lTable[i, 4], lTable[i, 5], lTable[i, 6], lTable[i, 7],
                    lTable[i, 8]
                });
        }

        FillClassification();
    }

    private void StartUpperLoop()
    {
        _mMasterVideoPlayer.isLooping = true;
        ChangeVideo(_mUpperLoopVideoClip);
    }

    private void StopUpper()
    {
        if (!_mGoal)
        {
            _mAnimator.SetTrigger(Bottom);
            ChangeVideo(_mBottomVideoClip);
            Invoke(nameof(StopBottom), (float) _mBottomVideoClip.length);
        }
    }

    private void StartGoalHome()
    {
        ChangeVideo(_mHomeGoalVideoClip);
        Invoke(nameof(StopGoal), (float) _mHomeGoalVideoClip.length);
        _mHomeScoreText.text = _mHomeScore.ToString();
    }

    private void StartGoalAway()
    {
        ChangeVideo(_mAwayGoalVideoClip);
        Invoke(nameof(StopGoal), (float) _mAwayGoalVideoClip.length);
        _mAwayScoreText.text = _mAwayScore.ToString();
    }

    private void StopGoal()
    {
        if (_mTimeout)
        {
            if (_mGoal)
                _mGoal = false;
            _mAnimator.SetBool(Upper, true);
            ChangeVideo(_mUpperIntroVideoClip);
            Invoke(nameof(StartUpperLoop), (float) _mUpperIntroVideoClip.length);
            _mTimeoutVideoPlayer.gameObject.SetActive(true);
            Invoke(nameof(StopTimeout), (float) _mTimeoutVideoPlayer.clip.length);
        }
        else
        {
            _mAnimator.SetTrigger(Bottom);
            ChangeVideo(_mBottomVideoClip);
            Invoke(nameof(StopBottom), (float) _mBottomVideoClip.length);
        }
    }

    private void StopTimeout()
    {
        _mTimeoutVideoPlayer.gameObject.SetActive(false);
        if (!_mStoppedTime)
            StartTime();
    }

    private void StartHalfTime()
    {
        mTableRenderer.gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        _mAnimator.SetBool(HalfTime, true);
        ChangeVideo(_mHalftimeIntroVideoClip);
        Invoke(nameof(StartHalfTimeLoop), (float) _mHalftimeIntroVideoClip.length);
    }

    private void StartHalfTimeLoop()
    {
        _mMasterVideoPlayer.isLooping = true;
        ChangeVideo(_mHalftimeLoopVideoClip);
        Invoke(nameof(StopHalfTimeLoop), (float) _mHalftimeLoopVideoClip.length);
    }

    private void StopHalfTimeLoop()
    {
        _mAnimator.SetBool(HalfTime, false);
        _mMasterVideoPlayer.isLooping = false;
        if (!halfTablePlayed)
            HalfTable();
        else
        {
            ChangeVideo(_mHalfTimeOutroVideoClip);
            Invoke(nameof(StopVideo), (float) _mHalfTimeOutroVideoClip.length);
        }
    }

    private void StartEndTime()
    {
        _mAnimator.SetTrigger(EndTime);
        ChangeVideo(_mEndMatchVideoClip);
        Invoke(nameof(StopEndTime), (float) _mEndMatchVideoClip.length);
    }

    private void StopEndTime()
    {
        StopVideo();
        Invoke(nameof(EndTable), 5f);
    }

    private void DecreaseSong()
    {
        _mAudioSource.volume -= .05f;

        if (_mAudioSource.volume <= 0)
        {
            _mAudioSource.Stop();
            CancelInvoke(nameof(DecreaseSong));
        }
    }

    private void IncreaseSong()
    {
        _mAudioSource.clip = _mLoopAudioClip;
        _mAudioSource.loop = true;
        _mAudioSource.volume += .05f;

        if (_mAudioSource.volume >= 1)
        {
            CancelInvoke(nameof(IncreaseSong));
        }
    }

//        HALF CONTROL    
    private void ChangeHalf()
    {
        if (!_mFirstHalf && !_mSecondHalf)
        {
            _mFirstHalf = true;
        }
        else if (_mFirstHalf)
        {
            _mFirstHalf = false;
            _mSecondHalf = true;
        }

        ResetFaults();
    }

    private void ResetFaults()
    {
        _mHomeFaults = 0;
        _mAwayFaults = 0;

        foreach (var fault in _mHomeFaultsUI)
        {
            fault.SetActive(false);
        }

        foreach (var fault in _mAwayFaultsUI)
        {
            fault.SetActive(false);
        }
    }

    private bool EndFirstHalf()
    {
        return _mPlaying && _mTime / 60 >= _mMaxTime - 1 && _mFirstHalf;
    }

    private bool EndSecondHalf()
    {
        return _mPlaying && _mTime / 60 >= _mMaxTime * 2 - 1 && _mSecondHalf;
    }

//        VIDEO METHODS    
    private void StopVideo()
    {
        mTableRenderer.gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        _mMasterVideoPlayer.clip = _mDefaultVideoClip;
    }

    private void ChangeVideo(VideoClip lVideoClip)
    {
        _mMasterVideoPlayer.clip = lVideoClip;
        _mMasterVideoPlayer.Play();
    }

//        LOAD THINGS
    private void LoadAudio()
    {
        _mIntroAudioClip = Resources.Load<AudioClip>("Audio/Intro");
        if (_mIntroAudioClip == null)
        {
            Debug.LogError("Intro Audio not Found");
            return;
        }

        _mLoopAudioClip = Resources.Load<AudioClip>("Audio/Loop");
        if (_mLoopAudioClip == null)
        {
            Debug.LogError("Loop Audio not Found");
            return;
        }

//        mGoalAudioClip = Resources.Load<AudioClip>("Audio/Intro");
//        if (mGoalAudioClip == null)
//        {
//            Debug.LogError("Goal Audio not Found");
//            return;
//        }

        _mOutroAudioClip = Resources.Load<AudioClip>("Audio/Ending");
        if (_mOutroAudioClip == null)
        {
            Debug.LogError("Outro Audio not Found");
            return;
        }

        Debug.Log("All Audios Loaded");
    }

    private void LoadVideos()
    {
        _mIntroVideoClip = Resources.Load<VideoClip>("Videos/Untouchables/Intro");
        if (_mIntroVideoClip == null)
        {
            Debug.LogError("Intro Video not Found");
            return;
        }

        _mDefaultVideoClip = Resources.Load<VideoClip>("Videos/Untouchables/Default");
        if (_mDefaultVideoClip == null)
        {
            Debug.LogError("Default Video not Found");
            return;
        }

        _mOutroVideoClip = Resources.Load<VideoClip>("Videos/Untouchables/Outro");
        if (_mOutroVideoClip == null)
        {
            Debug.LogError("Outro Video not Found");
            return;
        }

        // _mConnectedVideoClip = Resources.Load<VideoClip>("Videos/Untouchables/Connected");
        // if (_mConnectedVideoClip == null)
        // {
        //     Debug.LogError("Connected Bluetooth Video not Found");
        //     return;
        // }
        //
        // _mDisconnectedVideoClip = Resources.Load<VideoClip>("Videos/Untouchables/Disconnected");
        // if (_mDisconnectedVideoClip == null)
        // {
        //     Debug.LogError("Disconnected Bluetooth Video not Found");
        //     return;
        // }

        _mBottomVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_bottom");
        if (_mBottomVideoClip == null)
        {
            Debug.LogError("Bottom Scoreboard Video not Found");
            return;
        }

        _mUpperIntroVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_upper_intro");
        if (_mUpperIntroVideoClip == null)
        {
            Debug.LogError("Upper Intro Scoreboard Video not Found");
            return;
        }

        _mUpperLoopVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_upper_loop");
        if (_mUpperLoopVideoClip == null)
        {
            Debug.LogError("Upper Loop Scoreboard Video not Found");
            return;
        }

        _mUpperOutroVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_upper_outro");
        if (_mUpperOutroVideoClip == null)
        {
            Debug.LogError("Upper Outro Scoreboard Video not Found");
            return;
        }

        _mPreMatchVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_pre");
        if (_mPreMatchVideoClip == null)
        {
            Debug.LogError("Pre Match Video not Found");
            return;
        }

        _mEndMatchVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_end");
        if (_mEndMatchVideoClip == null)
        {
            Debug.LogError("End Match Video not Found");
            return;
        }

        _mHalftimeIntroVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_half_intro");
        if (_mHalftimeIntroVideoClip == null)
        {
            Debug.LogError("Intro Half Time Video not Found");
            return;
        }

        _mHalftimeLoopVideoClip = Resources.Load<VideoClip>("Videos/Season/" +
                                                            SingletonMatchType.GetInstance().Match + "_half_loop");
        if (_mHalftimeLoopVideoClip == null)
        {
            Debug.LogError("Loop Half Time Video not Found");
            return;
        }

        _mHalfTimeOutroVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match +
                                      "_half_outro");
        if (_mHalfTimeOutroVideoClip == null)
        {
            Debug.LogError("Outro Half Time Video not Found");
            return;
        }

        _mHomeGoalVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_goal_home");
        if (_mHomeGoalVideoClip == null)
        {
            Debug.LogError("Home Goal Video not Found");
            return;
        }

        _mAwayGoalVideoClip =
            Resources.Load<VideoClip>("Videos/Season/" + SingletonMatchType.GetInstance().Match + "_goal_away");
        if (_mAwayGoalVideoClip == null)
        {
            Debug.LogError("Away Goal Video not Found");
            return;
        }

        mPlaying = new VideoClip[16];
        mNotPlaying = new VideoClip[16];
        mRatxa = new VideoClip[4];

        var lPlaying = Resources.LoadAll("Videos/Table/Playing", typeof(VideoClip)).Cast<VideoClip>();
        int lCounter = 0;
        foreach (var VARIABLE in lPlaying)
        {
            mPlaying[lCounter] = VARIABLE;
            lCounter++;
        }

        var lNotPlaying = Resources.LoadAll("Videos/Table/Not Playing", typeof(VideoClip)).Cast<VideoClip>();
        lCounter = 0;
        foreach (var VARIABLE in lNotPlaying)
        {
            mNotPlaying[lCounter] = VARIABLE;
            lCounter++;
        }

        var lRatxa = Resources.LoadAll("Videos/Table/Streak", typeof(VideoClip)).Cast<VideoClip>();
        lCounter = 0;
        foreach (var VARIABLE in lRatxa)
        {
            mRatxa[lCounter] = VARIABLE;
            lCounter++;
        }

        _mTableVideoClip = Resources.Load<VideoClip>("Videos/Table/table");

        Debug.Log("All Videos Loaded");
    }

    private void FindVideoPlayer()
    {
        _mMasterVideoPlayer = transform.GetChild(0).GetComponent<VideoPlayer>();
        _mTimeoutVideoPlayer = transform.GetChild(1).GetComponent<VideoPlayer>();
    }

    private void FindTexts()
    {
        Transform parent = transform.GetChild(3);
        _mTimeText = parent.GetChild(0).GetComponent<Text>();
        _mHomeScoreText = parent.GetChild(1).GetComponent<Text>();
        _mAwayScoreText = parent.GetChild(2).GetComponent<Text>();

        _mHomeFaultsUI = new GameObject[parent.GetChild(4).childCount];
        int lCounter = 0;
        foreach (Transform lTransform in parent.GetChild(4))
        {
            lTransform.gameObject.SetActive(false);
            _mHomeFaultsUI[lCounter] = lTransform.gameObject;
            lCounter++;
        }

        _mAwayFaultsUI = new GameObject[parent.GetChild(5).childCount];
        lCounter = 0;
        foreach (Transform lTransform in parent.GetChild(5))
        {
            lTransform.gameObject.SetActive(false);
            _mAwayFaultsUI[lCounter] = lTransform.gameObject;
            lCounter++;
        }
    }

    private void FindTable()
    {
        mEquips = new VideoPlayer[16];
        mRatxa1 = new VideoPlayer[16];
        mRatxa2 = new VideoPlayer[16];
        mRatxa3 = new VideoPlayer[16];
        mRatxa4 = new VideoPlayer[16];

        mPunts = new Text[16];
        mGuanyats = new Text[16];
        mEmpatats = new Text[16];
        mPerduts = new Text[16];
        mJugats = new Text[16];

        MeshRenderer[] lEquips = new MeshRenderer[16];
        MeshRenderer[] lRatxa1 = new MeshRenderer[16];
        MeshRenderer[] lRatxa2 = new MeshRenderer[16];
        MeshRenderer[] lRatxa3 = new MeshRenderer[16];
        MeshRenderer[] lRatxa4 = new MeshRenderer[16];

        int lCounter = 0;

        foreach (Transform lTransform in mTableRenderer.GetChild(0))
        {
            lEquips[lCounter] = lTransform.GetComponent<MeshRenderer>();
            lRatxa1[lCounter] = lTransform.GetChild(0).GetComponent<MeshRenderer>();
            lRatxa2[lCounter] = lTransform.GetChild(1).GetComponent<MeshRenderer>();
            lRatxa3[lCounter] = lTransform.GetChild(2).GetComponent<MeshRenderer>();
            lRatxa4[lCounter] = lTransform.GetChild(3).GetComponent<MeshRenderer>();
            lCounter++;
        }

        lCounter = 0;

        foreach (Transform lTransform in transform.GetChild(2))
        {
            mEquips[lCounter] = lTransform.GetComponent<VideoPlayer>();
            mEquips[lCounter].targetMaterialRenderer = lEquips[lCounter];
            mRatxa1[lCounter] = lTransform.GetChild(0).GetComponent<VideoPlayer>();
            mRatxa1[lCounter].targetMaterialRenderer = lRatxa1[lCounter];
            mRatxa2[lCounter] = lTransform.GetChild(1).GetComponent<VideoPlayer>();
            mRatxa2[lCounter].targetMaterialRenderer = lRatxa2[lCounter];
            mRatxa3[lCounter] = lTransform.GetChild(2).GetComponent<VideoPlayer>();
            mRatxa3[lCounter].targetMaterialRenderer = lRatxa3[lCounter];
            mRatxa4[lCounter] = lTransform.GetChild(3).GetComponent<VideoPlayer>();
            mRatxa4[lCounter].targetMaterialRenderer = lRatxa4[lCounter];
            lCounter++;
        }

        lCounter = 0;
        foreach (Transform lTransform in mTableRenderer.GetChild(1))
        {
            mPunts[lCounter] = lTransform.GetChild(0).GetComponent<Text>();
            mJugats[lCounter] = lTransform.GetChild(1).GetComponent<Text>();
            mGuanyats[lCounter] = lTransform.GetChild(2).GetComponent<Text>();
            mEmpatats[lCounter] = lTransform.GetChild(3).GetComponent<Text>();
            mPerduts[lCounter] = lTransform.GetChild(4).GetComponent<Text>();
            lCounter++;
        }
    }
}