using System;
using System.Collections.Generic;
using Scoreboard;
using UnityEngine;

public class TableGenerator
{
    private List<Team> table;
    private ScoreboardGUI scoreboardGui;

    public List<Team> Table => table;

    public TableGenerator(ScoreboardGUI scoreboardGui)
    {
        this.scoreboardGui = scoreboardGui;
        table = new List<Team>();
        var tableData = FileController.LoadTableURL(MatchConfig.GetInstance().TableURL);
        for (int x = 0; x < tableData.GetLength(0); x++)
        {
            string streak = "";

            // for (int i = 0; i < tableData.GetLength(1); i++)
            // {
            // Debug.Log(tableData[x, 8]);
            // }

            foreach (var c in tableData[x, 8])
                if (c == 'G' || c == 'E' || c == 'P' || c == 'N' || c == 'S')
                    streak += c;
            while (streak.Length < 5)
            {
                streak += "N";
            }

            var team = new Team(tableData[x, 0], Int32.Parse(tableData[x, 2]), Int32.Parse(tableData[x, 1]),
                Int32.Parse(tableData[x, 6]),
                Int32.Parse(tableData[x, 7]), Int32.Parse(tableData[x, 3]), Int32.Parse(tableData[x, 4]),
                Int32.Parse(tableData[x, 5]),
                streak);
            team.IsPlaying = PlayingTeam(team);
            table.Add(team);
        }

        if (Team.LastStreak != tableData.GetLength(0))
            MatchConfig.GetInstance().LastMatchDayPlayed--;
    }

    public void UpdateTable()
    {
        table.Sort((x, y) => x.CompareTo(y));
        ToGUI();
    }

    private bool PlayingTeam(Team team)
    {
        var name = team.Name.Replace("\n", "");
        return name.Equals(PlayerPrefs.GetString("Local Team")) ||
               name.Equals(PlayerPrefs.GetString("Rival Team"));
    }

    public static int FindTeam(string team)
    {
        var name = team.Replace("\n", "");
        name = name.Replace("\r", "");
        int lastValue = 0;
        for (int i = 0; i < PlayerPrefs.GetInt("Season Length"); i++)
        {
            if (name.Equals(PlayerPrefs.GetString("FindTeam " + i)))
            {
                return i;
            }

            lastValue = i;
        }
        
        return lastValue + 1;
    }

    public void ToGUI()
    {
        int counter = 0;

        foreach (var team in table)
        {
            Color color = team.IsPlaying ? scoreboardGui.playingColor : scoreboardGui.notPlayingColor;
            scoreboardGui.Teams[counter].url =
                scoreboardGui.TableVideosDict[team.IsPlaying ? "Playing" : "Not Playing"][FindTeam(team.Name)];
            scoreboardGui.Played[counter].text = team.MatchesPlayed.ToString();
            scoreboardGui.Played[counter].color = color;
            scoreboardGui.Points[counter].text = team.Points.ToString();
            scoreboardGui.Points[counter].color = color;
            scoreboardGui.Won[counter].text = team.Won.ToString();
            scoreboardGui.Won[counter].color = color;
            scoreboardGui.Drawn[counter].text = team.Drawn.ToString();
            scoreboardGui.Drawn[counter].color = color;
            scoreboardGui.Lost[counter].text = team.Lost.ToString();
            scoreboardGui.Lost[counter].color = color;
            int x = 0;
            foreach (var c in team.Streak)
            {
                if (c == 'G')
                    scoreboardGui.Streak[x, counter].material = scoreboardGui.streakMaterials[3];
                else if (c == 'E')
                    scoreboardGui.Streak[x, counter].material = scoreboardGui.streakMaterials[1];
                else if (c == 'P')
                    scoreboardGui.Streak[x, counter].material = scoreboardGui.streakMaterials[0];
                else if (c == 'N' || c == 'S')
                    scoreboardGui.Streak[x, counter].material = scoreboardGui.streakMaterials[2];
                x++;
            }

            counter++;
        }
    }
}