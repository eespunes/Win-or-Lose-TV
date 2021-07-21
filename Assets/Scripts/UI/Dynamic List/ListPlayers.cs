using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListPlayers : MonoBehaviour
{
    private RectTransform _content;
    [SerializeField] private TMP_InputField teamName;
    [SerializeField] private bool isHome;


    public void Awake()
    {
        _content = GetComponent<RectTransform>();
        _content.sizeDelta = new Vector2(0, _content.childCount * 40f);
    }

    public void Save()
    {
        string team = "";
        if (isHome)
        {
            if (_content == null)
                _content = GetComponent<RectTransform>();

            ItemPlayer[] itemPlayers = new ItemPlayer[_content.childCount];
            int i = 0;

            team += teamName.text;

            foreach (RectTransform child in _content)
            {
                itemPlayers[i] = child.GetComponent<ItemPlayer>();
                i++;
            }

            for (i = itemPlayers.Length - 1; i >= 0; i--)
            {
                if (itemPlayers[i].number.text != "")
                    team += "\t\t" + itemPlayers[i].name.text + " - " + itemPlayers[i].number.text;
            }

            team += "\t\t" + teamName.text;
        }
        else
        {
            team += teamName.text + "\t\t";
            foreach (RectTransform child in _content)
            {
                ItemPlayer player = child.GetComponent<ItemPlayer>();
                if (player.number.text != "")
                    team += player.number.text + " - " + player.name.text + "\t\t";
            }

            team += teamName.text;
        }

        PlayerPrefs.SetString(isHome ? "Home Players" : "Away Players", team);
    }

    public void Load()
    {
        if (_content == null)
            _content = GetComponent<RectTransform>();

        ItemPlayer[] itemPlayers = new ItemPlayer[_content.childCount];
        int i = 0;
        foreach (RectTransform child in _content)
        {
            itemPlayers[i] = child.GetComponent<ItemPlayer>();
            i++;
        }

        string team = PlayerPrefs.GetString(isHome ? "Home Players" : "Away Players");
        i = isHome ? itemPlayers.Length - 1 : 0;
        foreach (var player in team.Split('\t'))
        {
            if (player.Length > 0)
            {
                if (player.Contains("-"))
                {
                    string[] playerSplit = player.Split('-');
                    if (isHome)
                    {
                        itemPlayers[(itemPlayers.Length - 1)-i].number.text = playerSplit[1].Substring(1);
                        itemPlayers[(itemPlayers.Length - 1)-i].name.text =
                            playerSplit[0].Substring(0, playerSplit[0].Length - 1);
                        i--;
                    }
                    else
                    {
                        itemPlayers[i].number.text = playerSplit[0].Substring(0, playerSplit[0].Length - 1);
                        itemPlayers[i].name.text = playerSplit[1].Substring(1);
                        i++;
                    }
                }
                else
                    teamName.text = player;
            }
        }
    }
}