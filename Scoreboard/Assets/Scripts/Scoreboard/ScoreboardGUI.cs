using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScoreboardGUI : MonoBehaviour
{
    public Color notPlayingColor, playingColor;
    private TableGenerator tableGenerator;
    private Dictionary<string, VideoClip> videosDict;
    private Dictionary<string, VideoClip[]> tableVideosDict;
    private VideoPlayer[] teams;
    private VideoPlayer[,] streak;
    private Text[] points;

    private void Awake()
    {
        tableGenerator=new TableGenerator(this);
        videosDict = Loader.LoadVideos();
        tableVideosDict = Loader.LoadTableVideos();
        teams = new VideoPlayer[16];
        streak = new VideoPlayer[4,16];
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
        
    }
    
    
    private void FindVideoPlayer()
    {
        masterVideoPlayer = transform.GetChild(0).GetComponent<VideoPlayer>();
        timeoutVideoPlayer = transform.GetChild(1).GetComponent<VideoPlayer>();
    }

    private void FindTexts()
    {
        Transform parent = transform.GetChild(3); 
        timeText = parent.GetChild(0).GetComponent<Text>();
        homeScoreText = parent.GetChild(1).GetComponent<Text>();
        awayScoreText = parent.GetChild(2).GetComponent<Text>();
        homeFaultsUI = new GameObject[parent.GetChild(4).childCount];
        int lCounter = 0;
        foreach (Transform lTransform in parent.GetChild(4))
        {
            lTransform.gameObject.SetActive(false);
            homeFaultsUI[lCounter] = lTransform.gameObject;
            lCounter++;
        }

        awayFaultsUI = new GameObject[parent.GetChild(5).childCount];
        lCounter = 0;
        foreach (Transform lTransform in parent.GetChild(5))
        {
            lTransform.gameObject.SetActive(false);
            awayFaultsUI[lCounter] = lTransform.gameObject;
            lCounter++;
        }
    }

    private void FindTable()
    {
        MeshRenderer[] teams = new MeshRenderer[16];
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
            teams[lCounter] = lTransform.GetComponent<VideoPlayer>();
            teams[lCounter].targetMaterialRenderer = lEquips[lCounter];
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
        foreach (Transform lTransform in tableRenderer.GetChild(1))
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
}