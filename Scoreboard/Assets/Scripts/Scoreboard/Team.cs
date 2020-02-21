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
    private int playingFaults;
    private string streak;
    private int streakPlace;

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
        playingFaults = 0;
        streakPlace = streak.LastIndexOf("N");
    }

    // GOALS CONTROLLER
    public void IncreaseScore(Team team)
    {
        ResetValues(team);

        playingScore++;

        AddValues(team);
    }

    public void DecreaseScore(Team team)
    {
        if (playingScore > 0)
        {
            ResetValues(team);

            playingScore--;

            AddValues(team);
        }
    }

    private void ResetValues(Team team)
    {
        if (playingScore > team.PlayingScore)
        {
            points -= 3;
            won--;
            team.lost--;
        }
        else if (playingScore == team.PlayingScore)
        {
            points--;
            drawn--;
            team.points--;
            team.drawn--;
        }
        else
        {
            lost--;
            team.won--;
            team.points -= 3;
        }

        goalsFor -= playingScore;
        goalsAgainst -= team.playingScore;
        team.goalsFor -= team.playingScore;
        team.goalsAgainst -= playingScore;
    }

    private void AddValues(Team team)
    {
        if (playingScore > team.PlayingScore)
        {
            points += 3;
            won++;
            team.lost++;
            streak = streak.Remove(streakPlace, 1).Insert(streakPlace, "G");
            team.Streak = team.Streak.Remove(streakPlace, 1).Insert(streakPlace, "P");
        }
        else if (playingScore == team.PlayingScore)
        {
            points++;
            drawn++;
            team.points++;
            team.drawn++;
            streak = streak.Remove(streakPlace, 1).Insert(streakPlace, "E");
            team.Streak = team.Streak.Remove(streakPlace, 1).Insert(streakPlace, "E");
        }
        else
        {
            lost++;
            team.points += 3;
            team.won++;
            streak = streak.Remove(streakPlace, 1).Insert(streakPlace, "P");
            team.Streak = team.Streak.Remove(streakPlace, 1).Insert(streakPlace, "G");
        }

        goalsFor += playingScore;
        goalsAgainst += team.playingScore;
        team.goalsFor += team.playingScore;
        team.goalsAgainst += playingScore;
    }

    // FAULTS CONTROLLER
    public void IncreaseFault()
    {
        playingFaults++;
    }

    public void ResetFaults()
    {
        playingFaults = 0;
    }

    public void StartMatch()
    {
        drawn++;
        points++;
        matchsPlayed++;
        streak = streak.Remove(streakPlace, 1).Insert(streakPlace, "E");
    }
    
    // EXTRAS
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
    
    // GETTERS AND SETTERS
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

    public int PlayingFaults => playingFaults;
}