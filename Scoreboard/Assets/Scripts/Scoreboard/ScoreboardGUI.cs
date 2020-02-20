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

    private void Awake()
    {
        videosDict = Loader.LoadVideos();
        tableVideosDict = Loader.LoadTableVideos();
        InitTable();
        FindVideoPlayer();
        FindTexts();
        FindTable();
        matchController = new MatchController(this);
        ChangeVideo(videosDict["Default"]);
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
        if (Input.GetKeyDown(KeyCode.M))
            StartCoroutine(Intro());
    }

    IEnumerator Intro()
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


    private void FindVideoPlayer()
    {
        masterVideoPlayer = transform.GetChild(0).GetComponent<VideoPlayer>();
        timeoutVideoPlayer = transform.GetChild(1).GetComponent<VideoPlayer>();
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
}