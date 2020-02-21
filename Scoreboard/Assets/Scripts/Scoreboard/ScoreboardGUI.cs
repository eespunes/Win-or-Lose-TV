using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScoreboardGUI : MonoBehaviour
{
    public Color notPlayingColor, playingColor;

    private TableGenerator tableGenerator;
    private MatchController matchController;

    private Dictionary<string, VideoClip> videosDict;
    private Dictionary<string, VideoClip[]> tableVideosDict;

    private VideoPlayer[] teams;
    private VideoPlayer[,] streak;

    private Text[] points;
    private Text[] won;
    private Text[] drawn;
    private Text[] lost;
    private Text[] played;

    private VideoPlayer masterVideoPlayer;
    private VideoPlayer timeoutVideoPlayer;
    public Transform tableRenderer;
    private Text time;
    private Text homeScore;
    private Text awayScore;
    private GameObject[] homeFaults;
    private GameObject[] awayFaults;
    private Dictionary<string, AudioClip> audioDict;


    private Animator animator;
    private static readonly int Bottom = Animator.StringToHash("Bottom");
    private static readonly int Upper = Animator.StringToHash("Upper");
    private static readonly int HalfTime = Animator.StringToHash("Half Time");
    private static readonly int EndTime = Animator.StringToHash("End Match");
    private static readonly int ToUpperIntro = Animator.StringToHash("To Upper Intro");
    private static readonly int ToIdle = Animator.StringToHash("To Idle");

    private void Awake()
    {
        animator = transform.GetChild(3).GetComponent<Animator>();
        videosDict = Loader.LoadVideos();
        tableVideosDict = Loader.LoadTableVideos();
        audioDict = Loader.LoadAudios();
        InitTable();
        FindVideoPlayer();
        FindTexts();
        FindTable();
        Team home = null, away = null;
        foreach (var team in tableGenerator.Table)
        {
            if (team.IsPlaying)
                if (team.Name.Replace("\n", "")
                    .Equals(MatchConfig.GetInstance().MatchDict[MatchConfig.GetInstance().Match][0]))
                    home = team;
                else
                    away = team;
        }

        matchController = new MatchController(this, home, away);
        ChangeVideo(videosDict["Default"]);

        timeoutVideoPlayer.clip = videosDict["Timeout"];
    }

    private void InitTable()
    {
        tableGenerator = new TableGenerator(this);
        teams = new VideoPlayer[16];
        streak = new VideoPlayer[4, 16];
        points = new Text[16];
        won = new Text[16];
        drawn = new Text[16];
        lost = new Text[16];
        played = new Text[16];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !matchController.Playing)
            StartCoroutine(PreMatch());
        matchController.Update();
    }

    private IEnumerator PreMatch()
    {
        tableGenerator.ToGUI();
        ChangeVideo(videosDict["Intro"]);
        yield return new WaitForSeconds((float) videosDict["Intro"].length);

        ChangeVideo(videosDict["Default"]);
        yield return new WaitForSeconds(5);

        ChangeVideo(videosDict["Pre Match"]);
        yield return new WaitForSeconds((float) videosDict["Pre Match"].length);

        ChangeVideo(videosDict["Default"]);
        yield return new WaitForSeconds(5);

        ChangeVideo(videosDict["Table"]);
        tableRenderer.gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds((float) videosDict["Table"].length);

        ChangeVideo(videosDict["Default"]);
        tableRenderer.gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public IEnumerator HalfMatch()
    {
        yield return new WaitForSeconds((float) (videosDict["Bottom"].length + 5));

        tableGenerator.UpdateTable();
        tableGenerator.ToGUI();
        animator.SetBool(HalfTime, true);
        ChangeVideo(videosDict["Half Intro"]);
        yield return new WaitForSeconds((float) videosDict["Half Intro"].length);

        ChangeVideo(videosDict["Half Loop"]);
        yield return new WaitForSeconds((float) videosDict["Half Loop"].length);
        animator.SetBool(HalfTime, false);
        ChangeVideo(videosDict["Table"]);
        tableRenderer.gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds((float) videosDict["Table"].length);

        tableRenderer.gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        animator.SetBool(HalfTime, true);
        ChangeVideo(videosDict["Half Intro"]);
        yield return new WaitForSeconds((float) videosDict["Half Intro"].length);

        ChangeVideo(videosDict["Half Loop"]);
        yield return new WaitForSeconds((float) videosDict["Half Loop"].length);

        ChangeVideo(videosDict["Half Outro"]);
        yield return new WaitForSeconds((float) videosDict["Half Outro"].length);
        animator.SetBool(HalfTime, false);

        ChangeVideo(videosDict["Default"]);
    }

    public IEnumerator EndMatch()
    {
        yield return new WaitForSeconds((float) (videosDict["Bottom"].length + 5));

        tableGenerator.UpdateTable();
        tableGenerator.ToGUI();
        animator.SetBool(EndTime, true);
        ChangeVideo(videosDict["End Match"]);
        yield return new WaitForSeconds((float) videosDict["End Match"].length);

        animator.SetBool(EndTime, false);
        ChangeVideo(videosDict["Default"]);
        yield return new WaitForSeconds(5);

        ChangeVideo(videosDict["Table"]);
        tableRenderer.gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds((float) videosDict["Table"].length);

        tableRenderer.gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        ChangeVideo(videosDict["Default"]);
        yield return new WaitForSeconds(5);

        ChangeVideo(videosDict["Outro"]);
        yield return new WaitForSeconds((float) videosDict["Outro"].length);
        ChangeVideo(videosDict["Default"]);
    }


    private void FindVideoPlayer()
    {
        masterVideoPlayer = transform.GetChild(0).GetComponent<VideoPlayer>();
        timeoutVideoPlayer = transform.GetChild(1).GetComponent<VideoPlayer>();
    }

    public IEnumerator StopUpperOrStartBottom(bool stopped)
    {
        // StartCoroutine(DecreaseSong);
        if (masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_upper_loop"))
        {
            if (!stopped)
            {
                animator.SetBool(Upper, false);
                masterVideoPlayer.isLooping = false;
                ChangeVideo(videosDict["Upper Outro"]);
                yield return new WaitForSeconds((float) videosDict["Upper Outro"].length);
                ChangeVideo(videosDict["Default"]);

                animator.SetTrigger(Bottom);
                ChangeVideo(videosDict["Bottom"]);
                yield return new WaitForSeconds((float) videosDict["Bottom"].length);

                ChangeVideo(videosDict["Default"]);
            }
        }
        else if (!(masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_bottom")))
        {
            animator.SetTrigger(Bottom);
            ChangeVideo(videosDict["Bottom"]);
            yield return new WaitForSeconds((float) videosDict["Bottom"].length);

            ChangeVideo(videosDict["Default"]);
            if (matchController.Playing || matchController.Timeout || MatchConfig.GetInstance().StoppedTime)
            {
                animator.SetBool(Upper, true);
                ChangeVideo(videosDict["Upper Intro"]);
                yield return new WaitForSeconds((float) videosDict["Upper Intro"].length);
                masterVideoPlayer.isLooping = true;
                ChangeVideo(videosDict["Upper Loop"]);
            }
        }
    }


    private void FindTexts()
    {
        Transform parent = transform.GetChild(3);
        time = parent.GetChild(0).GetComponent<Text>();
        homeScore = parent.GetChild(1).GetComponent<Text>();
        awayScore = parent.GetChild(2).GetComponent<Text>();
        homeFaults = new GameObject[parent.GetChild(4).childCount];
        int lCounter = 0;
        foreach (Transform lTransform in parent.GetChild(4))
        {
            lTransform.gameObject.SetActive(false);
            homeFaults[lCounter] = lTransform.gameObject;
            lCounter++;
        }

        awayFaults = new GameObject[parent.GetChild(5).childCount];
        lCounter = 0;
        foreach (Transform lTransform in parent.GetChild(5))
        {
            lTransform.gameObject.SetActive(false);
            awayFaults[lCounter] = lTransform.gameObject;
            lCounter++;
        }
    }

    private void FindTable()
    {
        MeshRenderer[] teams = new MeshRenderer[tableVideosDict["Playing"].Length];
        MeshRenderer[] streak1 = new MeshRenderer[tableVideosDict["Playing"].Length];
        MeshRenderer[] streak2 = new MeshRenderer[tableVideosDict["Playing"].Length];
        MeshRenderer[] streak3 = new MeshRenderer[tableVideosDict["Playing"].Length];
        MeshRenderer[] streak4 = new MeshRenderer[tableVideosDict["Playing"].Length];

        int counter = 0;

        foreach (Transform lTransform in tableRenderer.GetChild(0))
        {
            teams[counter] = lTransform.GetComponent<MeshRenderer>();
            streak1[counter] = lTransform.GetChild(0).GetComponent<MeshRenderer>();
            streak2[counter] = lTransform.GetChild(1).GetComponent<MeshRenderer>();
            streak3[counter] = lTransform.GetChild(2).GetComponent<MeshRenderer>();
            streak4[counter] = lTransform.GetChild(3).GetComponent<MeshRenderer>();
            counter++;
        }

        counter = 0;

        foreach (Transform lTransform in transform.GetChild(2))
        {
            this.teams[counter] = lTransform.GetComponent<VideoPlayer>();
            this.teams[counter].targetMaterialRenderer = teams[counter];
            this.streak[0, counter] = lTransform.GetChild(0).GetComponent<VideoPlayer>();
            this.streak[0, counter].targetMaterialRenderer = streak1[counter];
            this.streak[1, counter] = lTransform.GetChild(1).GetComponent<VideoPlayer>();
            this.streak[1, counter].targetMaterialRenderer = streak2[counter];
            this.streak[2, counter] = lTransform.GetChild(2).GetComponent<VideoPlayer>();
            this.streak[2, counter].targetMaterialRenderer = streak3[counter];
            this.streak[3, counter] = lTransform.GetChild(3).GetComponent<VideoPlayer>();
            this.streak[3, counter].targetMaterialRenderer = streak4[counter];
            counter++;
        }

        counter = 0;
        foreach (Transform lTransform in tableRenderer.GetChild(1))
        {
            points[counter] = lTransform.GetChild(0).GetComponent<Text>();
            played[counter] = lTransform.GetChild(1).GetComponent<Text>();
            won[counter] = lTransform.GetChild(2).GetComponent<Text>();
            drawn[counter] = lTransform.GetChild(3).GetComponent<Text>();
            lost[counter] = lTransform.GetChild(4).GetComponent<Text>();
            counter++;
        }
    }

    public void ChangeVideo(VideoClip videoClip)
    {
        masterVideoPlayer.clip = videoClip;
        masterVideoPlayer.Play();
    }

    public Dictionary<string, VideoClip> VideosDict => videosDict;

    public Dictionary<string, VideoClip[]> TableVideosDict => tableVideosDict;

    public VideoPlayer[] Teams => teams;

    public VideoPlayer[,] Streak => streak;

    public Text[] Points => points;

    public Text[] Won => won;

    public Text[] Drawn => drawn;

    public Text[] Lost => lost;

    public Text[] Played => played;

    public Text Time => time;

    public Text HomeScore => homeScore;

    public Text AwayScore => awayScore;

    public IEnumerator Goal(string video)
    {
        if (masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_upper_loop"))
        {
            animator.SetBool(Upper, false);
            masterVideoPlayer.isLooping = false;
            ChangeVideo(videosDict["Upper Outro"]);
            yield return new WaitForSeconds((float) videosDict["Upper Outro"].length);
        }
        else if (masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_bottom"))
        {
            yield return new WaitForSeconds((float) (videosDict["Bottom"].length - masterVideoPlayer.time));
        }

        ChangeVideo(videosDict["Default"]);

        animator.SetTrigger(ToIdle);
        ChangeVideo(videosDict[video]);
        yield return new WaitForSeconds((float) videosDict[video].length);

        ChangeVideo(videosDict["Default"]);

        StartCoroutine(StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
    }

    public IEnumerator Timeout()
    {
        if (masterVideoPlayer.isPlaying && !masterVideoPlayer.clip.name.Contains("_upper_loop"))
        {
            yield return new WaitForSeconds((float) (masterVideoPlayer.clip.length - masterVideoPlayer.time));
            ChangeVideo(videosDict["Default"]);
            animator.SetBool(Upper, true);
            ChangeVideo(videosDict["Upper Intro"]);
            yield return new WaitForSeconds((float) videosDict["Upper Intro"].length);
            masterVideoPlayer.isLooping = true;
            ChangeVideo(videosDict["Upper Loop"]);
        }

        timeoutVideoPlayer.gameObject.SetActive(true);
        yield return new WaitForSeconds((float) videosDict["Timeout"].length);
        matchController.StopTimeout();
    }

    public IEnumerator HomeFaults(int faults)
    {
        if (faults == 1)
            homeFaults[faults - 1].SetActive(true);
        else if (faults <= 5)
        {
            homeFaults[faults - 1].SetActive(true);
            homeFaults[faults - 2].SetActive(false);
        }
        else
            yield break;


        if (!(masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_bottom")))
        {
            if (masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_upper_loop"))
                masterVideoPlayer.frame = 0;
            else
            {
                masterVideoPlayer.isLooping = false;
                animator.SetTrigger(ToUpperIntro);
                ChangeVideo(videosDict["Upper Intro"]);
                yield return new WaitForSeconds((float) videosDict["Upper Intro"].length);
                masterVideoPlayer.isLooping = true;
                ChangeVideo(videosDict["Upper Loop"]);
            }
        }
    }

    public IEnumerator AwayFaults(int faults)
    {
        if (faults == 1)
            awayFaults[faults - 1].SetActive(true);
        else if (faults <= 5)
        {
            awayFaults[faults - 1].SetActive(true);
            awayFaults[faults - 2].SetActive(false);
        }
        else
            yield break;


        if (!(masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_bottom")))
        {
            if (masterVideoPlayer.isPlaying && masterVideoPlayer.clip.name.Contains("_upper_loop"))
                masterVideoPlayer.frame = 0;
            else
            {
                masterVideoPlayer.isLooping = false;
                animator.SetTrigger(ToUpperIntro);
                ChangeVideo(videosDict["Upper Intro"]);
                yield return new WaitForSeconds((float) videosDict["Upper Intro"].length);
                masterVideoPlayer.isLooping = true;
                ChangeVideo(videosDict["Upper Loop"]);
            }
        }
    }
}