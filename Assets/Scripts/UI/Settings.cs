using System;
using System.Collections;
using System.Collections.Generic;
using Scoreboard;
using SimpleFileBrowser;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject screenConfig;
    [SerializeField] private GameObject teamsConfig;
    [SerializeField] private GameObject matchConfig;
    [SerializeField] private GameObject seasonConfig;
    [SerializeField] private GameObject videosConfig;

    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject cancelButton;

    [SerializeField] private GameObject generateButton;
    [SerializeField] private TMP_InputField generateIF;

    private Dictionary<string, string> _configDict;

    [SerializeField] private TMP_InputField time, localTeam, results, table;
    [SerializeField] private Toggle stoppedTime;
    [SerializeField] private Toggle showTable;

    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private ListVideo listVideo;

    [SerializeField] private ListPlayers listPlayersHome;
    [SerializeField] private ListPlayers listPlayersAway;
    private string _folder;

    private void Start()
    {
        time.text = PlayerPrefs.GetInt("Maximum Time").ToString();
        localTeam.text = PlayerPrefs.GetString("Local Team");

        stoppedTime.isOn = PlayerPrefs.GetString("Stopped Time").Length > 0 &&
                           bool.Parse(PlayerPrefs.GetString("Stopped Time"));
        showTable.isOn = PlayerPrefs.GetString("Show Table").Length > 0 &&
                         !bool.Parse(PlayerPrefs.GetString("Show Table"));

        table.text = PlayerPrefs.GetString("Table URL");
        results.text = PlayerPrefs.GetString("Results URL");

        generateIF.text = PlayerPrefs.GetInt("Season Length").ToString();
        EnableDisableURls();
        EnableDisableGenerateButton();

        listPlayersHome.Load();
        listPlayersAway.Load();


        FileBrowser.SetFilters(true, new FileBrowser.Filter("Folder", ""));

        FileBrowser.SetDefaultFilter("");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }


    public void EnableDisableScreenConfig()
    {
        screenConfig.SetActive(!screenConfig.activeSelf);
        matchConfig.SetActive(false);
        seasonConfig.SetActive(false);
        videosConfig.SetActive(false);
        teamsConfig.SetActive(false);
        saveButton.SetActive(!screenConfig.activeSelf);
        cancelButton.SetActive(!screenConfig.activeSelf);
    }

    public void EnableDisableTeamsConfig()
    {
        teamsConfig.SetActive(!teamsConfig.activeSelf);
        matchConfig.SetActive(false);
        seasonConfig.SetActive(false);
        videosConfig.SetActive(false);
        screenConfig.SetActive(false);
        saveButton.SetActive(!teamsConfig.activeSelf);
        cancelButton.SetActive(!teamsConfig.activeSelf);
    }

    public void EnableDisableMatchConfig()
    {
        matchConfig.SetActive(!matchConfig.activeSelf);
        seasonConfig.SetActive(false);
        screenConfig.SetActive(false);
        videosConfig.SetActive(false);
        teamsConfig.SetActive(false);
        saveButton.SetActive(!matchConfig.activeSelf);
        cancelButton.SetActive(!matchConfig.activeSelf);
    }

    public void EnableDisableSeasonConfig()
    {
        seasonConfig.SetActive(!seasonConfig.activeSelf);
        matchConfig.SetActive(false);
        screenConfig.SetActive(false);
        videosConfig.SetActive(false);
        teamsConfig.SetActive(false);
        saveButton.SetActive(!seasonConfig.activeSelf);
        cancelButton.SetActive(!seasonConfig.activeSelf);
    }

    public void EnableDisableVideoConfig()
    {
        videosConfig.SetActive(!videosConfig.activeSelf);
        if (videosConfig.activeSelf)
        {
            StartCoroutine(ShowLoadDialogCoroutine());
        }
        else
        {
            StopCoroutine(ShowLoadDialogCoroutine());
            FileBrowser.HideDialog();
        }

        matchConfig.SetActive(false);
        seasonConfig.SetActive(false);
        screenConfig.SetActive(false);
        teamsConfig.SetActive(false);
        saveButton.SetActive(!videosConfig.activeSelf);
        cancelButton.SetActive(!videosConfig.activeSelf);
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(true, false, null, "Load Folder", "Load");

        if (FileBrowser.Success)
        {
            string folder = "";
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                folder = FileBrowser.Result[i];
// #if !UNITY_EDITOR
//             CopyFolder(folder, 1, Application.dataPath);
// #endif
            this._folder = folder;

// #if !UNITY_EDITOR
            Invoke("StartCopyFolder", 1f);
// #endif
        }
    }

    public void StartCopyFolder()
    {
        CopyFolder(_folder,1,Application.dataPath);
    }

    
    private void CopyFolder(string folder, int tabsIdx, string path)
    {
        FileBrowser.HideDialog();
        string tabs = "";
        for (int i = 0; i < tabsIdx; i++)
        {
            tabs += "\t";
        }

        foreach (var file in FileBrowserHelpers.GetEntriesInDirectory(folder))
        {
            if (!file.Extension.Equals(".meta"))
            {
                listVideo.AddToList(tabs + file.Name);
                if (file.IsDirectory)
                {
                    if (!FileBrowserHelpers.DirectoryExists(path + file.Name))
                        FileBrowserHelpers.CreateFolderInDirectory(path, file.Name);
                    CopyFolder(file.Path, tabsIdx + 1, path + "/" + file.Name);
                }
                else
                {
                    if (FileBrowserHelpers.FileExists(path + file.Name))
                        FileBrowserHelpers.DeleteFile(path + file.Name);
                    FileBrowserHelpers.WriteCopyToFile(FileBrowserHelpers.CreateFileInDirectory(path, file.Name),
                        file.Path);
                }
            }
        }
    }

    public void EnableDisableGenerateButton()
    {
        generateButton.SetActive(generateIF.text.Length > 0);
    }

    public void EnableDisableURls()
    {
        table.transform.parent.gameObject.SetActive(!showTable.isOn);
        results.transform.parent.gameObject.SetActive(!showTable.isOn);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Maximum Time", int.Parse(time.text));

        PlayerPrefs.SetString("Local Team", localTeam.text);

        PlayerPrefs.SetString("Stopped Time", stoppedTime.isOn.ToString());
        PlayerPrefs.SetString("Show Table", (!showTable.isOn).ToString());

        PlayerPrefs.SetString("Table URL", table.text);
        PlayerPrefs.SetString("Results URL", results.text);

        if (generateIF.text.Length > 0)
            PlayerPrefs.SetInt("Season Length", int.Parse(generateIF.text));

        foreach (Transform transform in spawnPoint)
        {
            ItemSeason item = transform.GetComponent<ItemSeason>();
            if (item != null)
            {
                PlayerPrefs.SetString(item.matchDay.text + " Home", item.home.text);
                PlayerPrefs.SetString(item.matchDay.text + " Away", item.away.text);
            }
        }

        listPlayersHome.Save();
        listPlayersAway.Save();

        SceneManager.LoadScene("MainMenu");
    }

    public void Cancel()
    {
        SceneManager.LoadScene("MainMenu");
    }
}