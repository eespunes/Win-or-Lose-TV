using System;
using UnityEngine;

public class Team : IComparable<Team>
{
    private string name;
    private int matchsPlayed;
    private int points;
    private int goalsFor;
    private int goalsAgainst;
    private int won;
    private int drawn;
    private int lost;
    private bool isPlaying;
    private char homeVisitor;
    private int playingScore;
    private string streak;

    public Team(string name, int matchsPlayed, int points, int goalsFor, int goalsAgainst, int won, int drawn, int lost,
        string streak)
    {
        this.name = name;
        this.matchsPlayed = matchsPlayed;
        this.points = points;
        this.goalsFor = goalsFor;
        this.goalsAgainst = goalsAgainst;
        this.won = won;
        this.drawn = drawn;
        this.lost = lost;
        this.streak = streak;
        isPlaying = false;
        playingScore = 0;
    }

    //Getters and Setters
    public string Name
    {
        get => name;
        set => name = value;
    }

    public int MatchsPlayed
    {
        get => matchsPlayed;
        set => matchsPlayed = value;
    }

    public int Points
    {
        get => points;
        set => points = value;
    }

    public int GoalsFor
    {
        get => goalsFor;
        set => goalsFor = value;
    }

    public int GoalsAgainst
    {
        get => goalsAgainst;
        set => goalsAgainst = value;
    }

    public int Won
    {
        get => won;
        set => won = value;
    }

    public int Drawn
    {
        get => drawn;
        set => drawn = value;
    }

    public int Lost
    {
        get => lost;
        set => lost = value;
    }

    public string Streak
    {
        get => streak;
        set => streak = value;
    }

    public bool IsPlaying
    {
        get => isPlaying;
        set => isPlaying = value;
    }

    public int PlayingScore
    {
        get => playingScore;
    }

    public void IncreaseScore(Team team)
    {
        ChangeValues2(team);

        playingScore++;
        goalsFor++;

        ChangeValues(team);
    }

    private void ChangeValues2(Team team)
    {
        if (playingScore > team.PlayingScore)
        {
            points -= 3;
            won--;
        }
        else if (playingScore == team.PlayingScore)
        {
            points--;
            drawn--;
        }
        else
            lost--;
    }

    private void ChangeValues(Team team)
    {
        if (playingScore > team.PlayingScore)
        {
            points += 3;
            won++;
        }
        else if (playingScore == team.PlayingScore)
        {
            points++;
            drawn++;
        }
        else
            lost++;
    }

    public void DecreaseScore(Team team)
    {
        ChangeValues2(team);

        playingScore--;
        goalsFor--;

        ChangeValues(team);
    }

    public int CompareTo(Team obj)
    {
        if (this.points > obj.Points)
            return -1;
        if (this.points < obj.Points)
            return 1;
        if (this.goalsFor - this.goalsAgainst > obj.GoalsFor - obj.goalsAgainst)
            return -1;
        if (this.goalsFor - this.goalsAgainst < obj.GoalsFor - obj.goalsAgainst)
            return 1;
        if (this.won > obj.Won)
            return -1;
        if (this.won < obj.Won)
            return 1;
        if (this.lost < obj.Lost)
            return -1;
        if (this.lost > obj.Lost)
            return 1;
        return 0;
    }

    public override string ToString()
    {
        return name + ":\n" + "\tMatches Played:" + matchsPlayed + "\n\tPoints:" + points + "\n\tWon:" + won +
               "\n\tDrawn:" + drawn + "\n\tLost:" + lost + "\n\tGoals For:" + goalsFor + "\n\tGoals Againts:" +
               goalsAgainst + "\n\tStreak:" + streak;
    }
}