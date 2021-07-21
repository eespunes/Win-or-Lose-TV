using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scoreboard;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Windows.WebCam;

public class ScoreboardGUI : MonoBehaviour
{
    #region Variables

    [Header("Colors")] public Color notPlayingColor;
    public Color playingColor;
    [Header("Colors Copa Catalunya")] public Color copaColor1;
    public Color copaColor2;

    [Header("Colors Lliga")] public Color leagueColor1;
    public Color leagueColor2;

    private TableGenerator _tableGenerator;
    private MatchController _matchController;

    private Dictionary<string, string> _videosDict;
    private Dictionary<string, string> _sponsorVideosDict;
    public Dictionary<string, string[]> TableVideosDict { get; private set; }

    public VideoPlayer[] Teams { get; private set; }
    public MeshRenderer[,] Streak { get; private set; }

    public Text[] Points { get; private set; }
    public Text[] Won { get; private set; }
    public Text[] Drawn { get; private set; }
    public Text[] Lost { get; private set; }
    public Text[] Played { get; private set; }

    [Header("Audio Clips")] [SerializeField]
    private AudioClip introAudio;

    [SerializeField] private AudioClip loopAudio;
    [SerializeField] private AudioClip outroAudio;

    [Header("Texts")] [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI homeScore;
    [SerializeField] private TextMeshProUGUI awayScore;
    [SerializeField] private TextMeshProUGUI matchPart;
    [SerializeField] private TextMeshProUGUI playersHome;
    [SerializeField] private TextMeshProUGUI playersAway;
    [SerializeField] private TextMeshProUGUI barText;
    [SerializeField] private Transform homeFaultsTr;
    [SerializeField] private Transform awayFaultsTr;

    public TextMeshProUGUI Time
    {
        get => time;
        set => time = value;
    }

    public TextMeshProUGUI HomeScore
    {
        get => homeScore;
        set => homeScore = value;
    }

    public TextMeshProUGUI AwayScore
    {
        get => awayScore;
        set => awayScore = value;
    }

    public bool CanPressButton { get; private set; }

    [Header("VideoPlayers")] public VideoPlayer masterVideoPlayer;
    public VideoPlayer overlayVideoPlayer;
    public VideoPlayer sponsorVideoPlayer;

    [Header("Other")] public Transform tableRenderer;

    private GameObject[] _homeFaults;

    private GameObject[] _awayFaults;


    public Animator animator;
    private static readonly int Bottom = Animator.StringToHash("Bottom");
    private static readonly int Upper = Animator.StringToHash("Upper");
    private static readonly int EndTime = Animator.StringToHash("End Match");
    private static readonly int ToUpperIntro = Animator.StringToHash("To Upper Intro");
    private static readonly int ToIdle = Animator.StringToHash("To Idle");
    private static readonly int HomeGoal = Animator.StringToHash("Home Goal");
    private static readonly int AwayGoal = Animator.StringToHash("Away Goal");
    private static readonly int TimeoutAnim = Animator.StringToHash("Timeout");

    private AudioSource _audioSource;
    private bool _goal;
    private bool _timeout;
    private bool _stopVideo = false;
    private static readonly int PlayersHome = Animator.StringToHash("Players Home");
    private static readonly int PlayersAway = Animator.StringToHash("Players Away");
    public Material[] streakMaterials;

    public ScoreboardGUI(bool goal)
    {
        this._goal = goal;
    }

    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 60;
        Team home = null, away = null;
        _audioSource = GetComponent<AudioSource>();
        _videosDict = Loader.LoadVideos();
        _sponsorVideosDict = Loader.LoadSponsorVideos();
        FindTexts();
        if (MatchConfig.GetInstance().ShowTable)
        {
            TableVideosDict = Loader.LoadTableVideos();
            _tableGenerator = new TableGenerator(this);
            Teams = new VideoPlayer[16];
            Streak = new MeshRenderer[5, 16];
            Points = new Text[16];
            Won = new Text[16];
            Drawn = new Text[16];
            Lost = new Text[16];
            Played = new Text[16];
            FindTable();

            foreach (var team in _tableGenerator.Table)
            {
                if (team.IsPlaying)
                {
                    if (team.Name.Replace("\n", "")
                        .Equals(PlayerPrefs.GetString(MatchConfig.GetInstance().Match + " Home")))
                    {
                        home = team;
                    }
                    else
                    {
                        away = team;
                    }
                }
            }

            if (home != null && home.InitStreakPlace())
            {
                StringBuilder homeSB = new StringBuilder(home.Streak);
                homeSB[home.StreakPlace] = 'N';
                home.Streak = homeSB.ToString();
            }

            if (away != null && away.InitStreakPlace())
            {
                StringBuilder awaySB = new StringBuilder(away.Streak);
                awaySB[away.StreakPlace] = 'N';
                away.Streak = awaySB.ToString();
            }

            var resultData = FileController.LoadResultURL(
                MatchConfig.GetInstance().ResultURL + Int32.Parse(MatchConfig.GetInstance().Match.Replace("J", "")),
                home.Name);

            ResetTable(resultData, home, away);
            time.color = leagueColor2;
            homeScore.color = leagueColor1;
            awayScore.color = leagueColor1;
            matchPart.color = leagueColor1;
            playersHome.color = leagueColor2;
            playersAway.color = leagueColor2;
            barText.color = leagueColor2;
            barText.outlineWidth = .1f;
        }
        else
        {
            home = new Team(PlayerPrefs.GetString(MatchConfig.GetInstance().Match + " Home"));
            away = new Team(PlayerPrefs.GetString(MatchConfig.GetInstance().Match + " Away"));
            // time.color = copaColor2;
            // homeScore.color = copaColor1;
            // awayScore.color = copaColor1;
            // matchPart.color = copaColor1;
            // playersHome.color = copaColor2;
            // playersAway.color = copaColor2;
            // barText.color = copaColor2;
            // barText.outlineWidth = 0f;
            time.color = leagueColor2;
            homeScore.color = leagueColor1;
            awayScore.color = leagueColor1;
            matchPart.color = leagueColor1;
            playersHome.color = leagueColor2;
            playersAway.color = leagueColor2;
            barText.color = leagueColor2;
            barText.outlineWidth = .1f;
        }

        _matchController = new MatchController(this, home, away);
        ChangeVideo(_videosDict["Default"], null);

        overlayVideoPlayer.url = _videosDict["Timeout"];

        playersHome.text = PlayerPrefs.GetString("Home Players");
        playersAway.text = PlayerPrefs.GetString("Away Players");
    }

    private static void ResetTable(int[] resultData, Team home, Team away)
    {
        if (resultData != null)
        {
            home.GoalsAgainst -= resultData[1];
            home.GoalsFor -= resultData[0];
            home.MatchesPlayed--;
            away.GoalsAgainst -= resultData[0];
            away.GoalsFor -= resultData[1];
            away.MatchesPlayed--;

            if (resultData[0] > resultData[1])
            {
                home.Won--;
                home.Points -= 3;
                away.Lost--;
            }
            else if (resultData[0] < resultData[1])
            {
                home.Lost--;
                away.Points -= 3;
                away.Won--;
            }
            else
            {
                home.Drawn--;
                home.Points--;
                away.Points--;
                away.Drawn--;
            }
        }
    }

    private void Update()
    {
        _matchController.Update();
    }

    #region MatchFlow

    private IEnumerator WaitingToStopMasterVideoplayer()
    {
        yield return new WaitForSeconds(.5f);
        while (masterVideoPlayer.isPlaying)
            yield return null;
    }

    private IEnumerator WaitingToStopOverlapVideoplayer()
    {
        yield return new WaitForSeconds(.5f);
        while (overlayVideoPlayer.isPlaying)
            yield return null;
    }

    public IEnumerator PreMatch()
    {
        CanPressButton = false;
        matchPart.text = "1ª PARTE";
        if (MatchConfig.GetInstance().ShowTable)
            _tableGenerator.UpdateTable();

        _audioSource.clip = introAudio;
        _audioSource.loop = false;
        _audioSource.Play();
        InvokeRepeating(nameof(IncreaseSong), 0, .1f);

        ChangeVideo(_videosDict["Intro"], null);
        yield return WaitingToStopMasterVideoplayer();

        ChangeVideo(_videosDict["Default"], null);
        yield return new WaitForSeconds(3.5f);
        ChangeVideo(_videosDict["Pre Match"], _sponsorVideosDict["Pre Match"]);
        yield return WaitingToStopMasterVideoplayer();
        ChangeVideo(_videosDict["Default"], null);
        
        yield return new WaitForSeconds(4f);
        
        animator.SetBool(PlayersAway, true);
        ChangeVideo(_videosDict["Players Away"], _sponsorVideosDict["Players Away"]);
        yield return WaitingToStopMasterVideoplayer();
        
        ChangeVideo(_videosDict["Default"], null);
        yield return new WaitForSeconds(5);
        
        animator.SetBool(PlayersHome, true);
        ChangeVideo(_videosDict["Players Home"], _sponsorVideosDict["Players Home"]);
        yield return WaitingToStopMasterVideoplayer();
        
        ChangeVideo(_videosDict["Default"], null);
        
        if (MatchConfig.GetInstance().ShowTable)
        {
            yield return new WaitForSeconds(5);
            ChangeVideo(_videosDict["Table"], _sponsorVideosDict["Table"]);
            tableRenderer.gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            yield return WaitingToStopMasterVideoplayer();
            tableRenderer.gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }
        
        _matchController.IntroPressed = true;
        CanPressButton = true;
        ChangeVideo(_videosDict["Default"], null);
        
        yield return new WaitForSeconds((_audioSource.clip.samples - _audioSource.timeSamples) /
                                        (_audioSource.clip.frequency * 1f));
        
        // InvokeRepeating(nameof(IncreaseSong), 0, .1f);
        _audioSource.clip = loopAudio;
        _audioSource.loop = true;
        _audioSource.Play();
        
    }

    public IEnumerator HalfMatch()
    {
        _matchController.IntroPressed = false;
        CanPressButton = false;

        CancelInvoke(nameof(DecreaseSong));

        _audioSource.clip = loopAudio;
        _audioSource.loop = true;
        _audioSource.Play();

        InvokeRepeating(nameof(IncreaseSong), .05f, .1f);

        yield return new WaitForSeconds(30);
        if (MatchConfig.GetInstance().ShowTable)
            _tableGenerator.UpdateTable();

        animator.SetBool(EndTime, true);
        ChangeVideo(_videosDict["Half Time"], _sponsorVideosDict["Half Time"]);
        yield return WaitingToStopMasterVideoplayer();
        if (MatchConfig.GetInstance().ShowTable)
        {
            animator.SetBool(EndTime, false);
            ChangeVideo(_videosDict["Table"], _sponsorVideosDict["Table"]);
            tableRenderer.gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            yield return WaitingToStopMasterVideoplayer();

            tableRenderer.gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            animator.SetBool(EndTime, true);
            ChangeVideo(_videosDict["Half Time"], _sponsorVideosDict["Half Time"]);
            yield return WaitingToStopMasterVideoplayer();
        }

        matchPart.text = "2ª PARTE";
        _matchController.IntroPressed = true;
        CanPressButton = true;
        ChangeVideo(_videosDict["Default"], null);
    }

    public IEnumerator EndMatch()
    {
        _matchController.IntroPressed = false;
        CancelInvoke(nameof(DecreaseSong));

        _audioSource.clip = loopAudio;
        _audioSource.loop = true;
        _audioSource.Play();
        InvokeRepeating(nameof(IncreaseSong), .25f, .25f);

        yield return new WaitForSeconds(30);
        if (MatchConfig.GetInstance().ShowTable)
            _tableGenerator.UpdateTable();

        animator.SetBool(EndTime, true);
        ChangeVideo(_videosDict["End Match"], _sponsorVideosDict["End Match"]);
        yield return WaitingToStopMasterVideoplayer();

        animator.SetBool(EndTime, false);
        ChangeVideo(_videosDict["Default"], null);
        yield return new WaitForSeconds(5);
        if (MatchConfig.GetInstance().ShowTable)
        {
            ChangeVideo(_videosDict["Table"], _sponsorVideosDict["Table"]);
            tableRenderer.gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            yield return WaitingToStopMasterVideoplayer();

            tableRenderer.gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            ChangeVideo(_videosDict["Default"], null);
            yield return new WaitForSeconds(5);
        }

        _audioSource.clip = outroAudio;
        _audioSource.loop = false;
        _audioSource.Play();
        ChangeVideo(_videosDict["Outro"], null);
        yield return WaitingToStopMasterVideoplayer();
        ChangeVideo(_videosDict["Default"], null);

        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator StopUpperOrStartBottom(bool stopped)
    {
        InvokeRepeating(nameof(DecreaseSong), .25f, .25f);
        if (masterVideoPlayer.isPlaying && masterVideoPlayer.url.Contains("_upper_loop"))
        {
            if (!stopped)
            {
                StartCoroutine(UpperToBottom(stopped));
            }
            else
            {
                if ((_goal || _timeout) && _matchController.Playing)
                {
                    _goal = false;
                    _timeout = false;
                    StartCoroutine(UpperToBottom(stopped));
                }
                else if (_matchController.StartHalf)
                {
                    StartCoroutine(UpperToBottom(stopped));
                }
            }
        }
        else if (!(masterVideoPlayer.isPlaying && masterVideoPlayer.url.Contains("_bottom")))
        {
            animator.SetTrigger(Bottom);
            ChangeVideo(_videosDict["Bottom"], _sponsorVideosDict["Bottom"]);
            yield return WaitingToStopMasterVideoplayer();

            ChangeVideo(_videosDict["Default"], null);
            if (_matchController.Playing || _matchController.Timeout || MatchConfig.GetInstance().StoppedTime)
            {
                animator.SetBool(Upper, true);
                ChangeVideo(_videosDict["Upper Intro"], _sponsorVideosDict["Upper Intro"]);
                yield return WaitingToStopMasterVideoplayer();
                masterVideoPlayer.isLooping = true;
                ChangeVideo(_videosDict["Upper Loop"], _sponsorVideosDict["Upper Loop"]);
            }
        }
    }

    private IEnumerator UpperToBottom(bool stopped)
    {
        animator.SetBool(Upper, false);
        masterVideoPlayer.isLooping = false;
        ChangeVideo(_videosDict["Upper Outro"], _sponsorVideosDict["Upper Outro"]);
        yield return WaitingToStopMasterVideoplayer();
        ChangeVideo(_videosDict["Default"], null);

        animator.SetTrigger(Bottom);
        ChangeVideo(_videosDict["Bottom"], _sponsorVideosDict["Bottom"]);
        yield return WaitingToStopMasterVideoplayer();

        ChangeVideo(_videosDict["Default"], null);

        if (_matchController.Playing || (stopped && !_matchController.StartHalf))
        {
            animator.SetBool(Upper, true);
            ChangeVideo(_videosDict["Upper Intro"], _sponsorVideosDict["Upper Intro"]);
            yield return WaitingToStopMasterVideoplayer();
            masterVideoPlayer.isLooping = true;
            ChangeVideo(_videosDict["Upper Loop"], _sponsorVideosDict["Upper Loop"]);
        }
    }

    public IEnumerator Goal(string video)
    {
        CanPressButton = false;
        if (masterVideoPlayer.isPlaying && !masterVideoPlayer.url.Contains("_upper_loop"))
        {
            yield return WaitingToStopMasterVideoplayer();
            ChangeVideo(_videosDict["Default"], null);
            animator.SetBool(Upper, true);
            ChangeVideo(_videosDict["Upper Intro"], _sponsorVideosDict["Upper Intro"]);
            yield return WaitingToStopMasterVideoplayer();
            masterVideoPlayer.isLooping = true;
            ChangeVideo(_videosDict["Upper Loop"], _sponsorVideosDict["Upper Loop"]);
        }
        else
            masterVideoPlayer.frame = 0;

        bool isHomeTeam = video.Equals("Home Goal");

        if (isHomeTeam) animator.SetTrigger(HomeGoal);
        else animator.SetTrigger(AwayGoal);


        ChangeVideoOverlay(_videosDict[video], _sponsorVideosDict[video]);
        overlayVideoPlayer.gameObject.SetActive(true);
        overlayVideoPlayer.targetMaterialRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        if (isHomeTeam) _matchController.IncreaseScoreHomeUI();
        else _matchController.IncreaseScoreAwayUI();
        yield return WaitingToStopOverlapVideoplayer();
        overlayVideoPlayer.gameObject.SetActive(false);
        overlayVideoPlayer.targetMaterialRenderer.gameObject.SetActive(false);
        _goal = true;
        CanPressButton = true;
        StartCoroutine(StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
    }

    public IEnumerator Timeout()
    {
        CanPressButton = false;
        if (masterVideoPlayer.isPlaying && !masterVideoPlayer.url.Contains("_upper_loop"))
        {
            yield return WaitingToStopMasterVideoplayer();
            ChangeVideo(_videosDict["Default"], null);
            animator.SetBool(Upper, true);
            ChangeVideo(_videosDict["Upper Intro"], _sponsorVideosDict["Upper Intro"]);
            yield return WaitingToStopMasterVideoplayer();
            masterVideoPlayer.isLooping = true;
            ChangeVideo(_videosDict["Upper Loop"], _sponsorVideosDict["Upper Loop"]);
        }

        _timeout = true;
        ChangeVideoOverlay(_videosDict["Timeout"], _sponsorVideosDict["Timeout"]);
        _matchController.Playing = false;
        overlayVideoPlayer.gameObject.SetActive(true);
        overlayVideoPlayer.targetMaterialRenderer.gameObject.SetActive(true);
        animator.SetTrigger(TimeoutAnim);
        yield return WaitingToStopOverlapVideoplayer();
        overlayVideoPlayer.gameObject.SetActive(false);
        overlayVideoPlayer.targetMaterialRenderer.gameObject.SetActive(false);
        CanPressButton = true;
        _matchController.StopTimeout();
    }

    public IEnumerator HomeFaults(int faults)
    {
        if (faults == 1)
            _homeFaults[faults - 1].SetActive(true);
        else if (faults <= 5)
        {
            _homeFaults[faults - 1].SetActive(true);
            _homeFaults[faults - 2].SetActive(false);
        }
        else
            yield break;


        if (!(masterVideoPlayer.isPlaying && masterVideoPlayer.url.Contains("_bottom")))
        {
            if (masterVideoPlayer.isPlaying && masterVideoPlayer.url.Contains("_upper_loop"))
                masterVideoPlayer.frame = 0;
            else
            {
                masterVideoPlayer.isLooping = false;
                animator.SetTrigger(ToUpperIntro);
                ChangeVideo(_videosDict["Upper Intro"], _sponsorVideosDict["Upper Intro"]);
                yield return WaitingToStopMasterVideoplayer();
                masterVideoPlayer.isLooping = true;
                ChangeVideo(_videosDict["Upper Loop"], _sponsorVideosDict["Upper Loop"]);
            }
        }
    }

    public IEnumerator AwayFaults(int faults)
    {
        if (faults == 1)
            _awayFaults[faults - 1].SetActive(true);
        else if (faults <= 5)
        {
            _awayFaults[faults - 1].SetActive(true);
            _awayFaults[faults - 2].SetActive(false);
        }
        else
            yield break;


        if (!(masterVideoPlayer.isPlaying && masterVideoPlayer.url.Contains("_bottom")))
        {
            if (masterVideoPlayer.isPlaying && masterVideoPlayer.url.Contains("_upper_loop"))
                masterVideoPlayer.frame = 0;
            else
            {
                masterVideoPlayer.isLooping = false;
                animator.SetTrigger(ToUpperIntro);
                ChangeVideo(_videosDict["Upper Intro"], _sponsorVideosDict["Upper Intro"]);
                yield return WaitingToStopMasterVideoplayer();
                masterVideoPlayer.isLooping = true;
                ChangeVideo(_videosDict["Upper Loop"], _sponsorVideosDict["Upper Loop"]);
            }
        }
    }

    public void ResetFaults()
    {
        foreach (var homeFault in _homeFaults)
        {
            homeFault.SetActive(false);
        }

        foreach (var awayFault in _awayFaults)
        {
            awayFault.SetActive(false);
        }
    }

    #endregion

    #region VideoController

    public void ChangeVideo(string videoClip, string sponsorVideoClip)
    {
        if (sponsorVideoClip == null)
        {
            sponsorVideoPlayer.targetMaterialRenderer.gameObject.SetActive(false);
            sponsorVideoPlayer.gameObject.SetActive(false);
        }
        else
        {
            ChangeVideoSponsor(sponsorVideoClip);
        }

        masterVideoPlayer.url = videoClip;
        masterVideoPlayer.Play();
    }

    public void ChangeVideoOverlay(string videoClip, string sponsorVideoClip)
    {
        ChangeVideoSponsor(sponsorVideoClip);
        overlayVideoPlayer.url = videoClip;
        overlayVideoPlayer.Play();
    }

    public void ChangeVideoSponsor(string videoClip)
    {
        sponsorVideoPlayer.gameObject.SetActive(true);
        sponsorVideoPlayer.targetMaterialRenderer.gameObject.SetActive(true);
        sponsorVideoPlayer.url = videoClip;
        sponsorVideoPlayer.Play();
    }

    #endregion

    #region AudioControllers

    private void IncreaseSong()
    {
        _audioSource.volume += .05f;

        if (_audioSource.volume >= 1)
        {
            _audioSource.volume = 1;
            CancelInvoke(nameof(IncreaseSong));
        }
    }

    private void DecreaseSong()
    {
        _audioSource.volume -= .05f;

        if (_audioSource.volume <= 0)
        {
            _audioSource.volume = 0;
            _audioSource.Stop();
            CancelInvoke(nameof(DecreaseSong));
        }
    }

    #endregion

    #region Finders

    private void FindTexts()
    {
        Transform parent = transform.GetChild(3);
        _homeFaults = new GameObject[homeFaultsTr.childCount];
        int lCounter = 0;
        foreach (Transform transform in homeFaultsTr)
        {
            GameObject gameObject = transform.gameObject;
            gameObject.SetActive(false);
            _homeFaults[lCounter] = gameObject;
            lCounter++;
        }

        _awayFaults = new GameObject[awayFaultsTr.childCount];
        lCounter = 0;
        foreach (Transform transform in awayFaultsTr)
        {
            GameObject gameObject = transform.gameObject;
            gameObject.SetActive(false);
            _awayFaults[lCounter] = gameObject;
            lCounter++;
        }
    }

    private void FindTable()
    {
        MeshRenderer[] teams = new MeshRenderer[TableVideosDict["Playing"].Length];
        MeshRenderer[] streak1 = new MeshRenderer[TableVideosDict["Playing"].Length];
        MeshRenderer[] streak2 = new MeshRenderer[TableVideosDict["Playing"].Length];
        MeshRenderer[] streak3 = new MeshRenderer[TableVideosDict["Playing"].Length];
        MeshRenderer[] streak4 = new MeshRenderer[TableVideosDict["Playing"].Length];
        MeshRenderer[] streak5 = new MeshRenderer[TableVideosDict["Playing"].Length];

        int counter = 0;

        foreach (Transform lTransform in tableRenderer.GetChild(0))
        {
            teams[counter] = lTransform.GetComponent<MeshRenderer>();
            streak1[counter] = lTransform.GetChild(0).GetComponent<MeshRenderer>();
            streak2[counter] = lTransform.GetChild(1).GetComponent<MeshRenderer>();
            streak3[counter] = lTransform.GetChild(2).GetComponent<MeshRenderer>();
            streak4[counter] = lTransform.GetChild(3).GetComponent<MeshRenderer>();
            streak5[counter] = lTransform.GetChild(4).GetComponent<MeshRenderer>();
            counter++;
        }

        counter = 0;

        foreach (Transform lTransform in transform.GetChild(3))
        {
            this.Teams[counter] = lTransform.GetComponent<VideoPlayer>();
            this.Teams[counter].targetMaterialRenderer = teams[counter];

            this.Streak[0, counter] = streak1[counter];
            this.Streak[1, counter] = streak2[counter];
            this.Streak[2, counter] = streak3[counter];
            this.Streak[3, counter] = streak4[counter];
            this.Streak[4, counter] = streak5[counter];
            counter++;
        }

        counter = 0;
        foreach (Transform lTransform in tableRenderer.GetChild(1))
        {
            Played[counter] = lTransform.GetChild(0).GetComponent<Text>();
            Lost[counter] = lTransform.GetChild(1).GetComponent<Text>();
            Drawn[counter] = lTransform.GetChild(2).GetComponent<Text>();
            Won[counter] = lTransform.GetChild(3).GetComponent<Text>();
            Points[counter] = lTransform.GetChild(4).GetComponent<Text>();
            counter++;
        }
    }

    #endregion
}