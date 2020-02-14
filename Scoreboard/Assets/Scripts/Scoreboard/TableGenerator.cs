using System;
using System.Collections;
using Boo.Lang;
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
        var data = FileReader.LoadURL(SingletonMatchType.GetInstance().GroupURL);

        for (int x = 0; x < data.GetLength(0); x++)
        {
            string streak = "";
            foreach (var c in data[x, 8])
                if (c == 'G' || c == 'E' || c == 'P' || c == 'N' || c == 'S')
                    streak += c;
            var team = new Team(data[x, 0], Int32.Parse(data[x, 2]), Int32.Parse(data[x, 1]),
                Int32.Parse(data[x, 6]),
                Int32.Parse(data[x, 7]), Int32.Parse(data[x, 3]), Int32.Parse(data[x, 4]), Int32.Parse(data[x, 5]),
                streak);
            team.IsPlaying = PlayingTeam(team);
            table.Add(team);
        }
    }

    public void UpdateTable()
    {
        table = table.Sort((x, y) => x.CompareTo(y));
    }

    private bool PlayingTeam(Team team)
    {
        var dict = FileReader.LoadFileToDictionary("season");
        var name = team.Name.Replace("\n", "");
        return name.Equals(dict[SingletonMatchType.GetInstance().Match][0]) ||
               name.Equals(dict[SingletonMatchType.GetInstance().Match][1]);
    }

    private int FindTeam(Team team)
    {
        var name = team.Name.Replace("\n", "");
        switch (name)
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

    private void ToGUI()
    {
        int counter = 0;
        foreach (var team in table)
        {
            Color color = team.IsPlaying ? scoreboardGui.playingColor : scoreboardGui.notPlayingColor;
            // mEquips[lCounter].clip = mPlaying[FindTeam(team)];
            mJugats[counter].text = team.MatchsPlayed;
            mJugats[counter].color = color;
            mPunts[counter].text = team.Points;
            mPunts[counter].color = color;
            mGuanyats[counter].text = team.Won;
            mGuanyats[counter].color = color;
            mEmpatats[counter].text = team.Drawn;
            mEmpatats[counter].color = color;
            mPerduts[counter].text = team.Lost;
            mPerduts[counter].color = color;
            counter++;
        }
    }
}