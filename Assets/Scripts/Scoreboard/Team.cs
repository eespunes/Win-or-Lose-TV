using System;
using UnityEngine;

public class Team : IComparable<Team>
{
    public static int LastStreak = 0;
    private char _homeVisitor;

    public int StreakPlace { get; set; }
    public string Name { get; set; }

    public int MatchesPlayed { get; set; }

    public int Points { get; set; }

    public int GoalsFor { get; set; }

    public int GoalsAgainst { get; set; }

    public int Won { get; set; }

    public int Drawn { get; set; }

    public int Lost { get; set; }

    public string Streak { get; set; }

    public bool IsPlaying { get; set; }

    public int PlayingScore { get; private set; }

    public int PlayingFaults { get; private set; }

    public Team(string name, int matchesPlayed, int points, int goalsFor, int goalsAgainst, int won, int drawn,
        int lost,
        string streak)
    {
        this.Name = name;
        this.MatchesPlayed = matchesPlayed;
        this.Points = points;
        this.GoalsFor = goalsFor;
        this.GoalsAgainst = goalsAgainst;
        this.Won = won;
        this.Drawn = drawn;
        this.Lost = lost;
        this.Streak = streak;
        IsPlaying = false;
        PlayingScore = 0;
        PlayingFaults = 0;
        StreakPlace = streak.LastIndexOf("N") < 0 ? streak.LastIndexOf("S") : streak.LastIndexOf("N");

        if (StreakPlace == 4)
            LastStreak++;
    }

    public Team(string name)
    {
        this.Name = name;
        this.MatchesPlayed = 0;
        this.Points = 0;
        this.GoalsFor = 0;
        this.GoalsAgainst = 0;
        this.Won = 0;
        this.Drawn = 0;
        this.Lost = 0;
        this.Streak = "";
        IsPlaying = true;
        PlayingScore = 0;
        PlayingFaults = 0;

        if (StreakPlace == 4)
            LastStreak++;
    }

    public bool InitStreakPlace()
    {
        int match = Int32.Parse(MatchConfig.GetInstance().Match.Replace("J", ""));
        if (MatchConfig.GetInstance().LastMatchDayPlayed <= 5 && match <= 5)
            StreakPlace = match - 1;
        else
            StreakPlace = 4 - (MatchConfig.GetInstance().LastMatchDayPlayed -
                               match);
        return StreakPlace <= 4 && StreakPlace >= 0;
    }

    // GOALS CONTROLLER
    public void IncreaseScore(Team team)
    {
        if (MatchConfig.GetInstance().ShowTable)
            ResetValues(team);

        PlayingScore++;
        
        if (MatchConfig.GetInstance().ShowTable)
            AddValues(team);
    }

    public void DecreaseScore(Team team)
    {
        if (PlayingScore > 0)
        {
            if (MatchConfig.GetInstance().ShowTable)
                ResetValues(team);

            PlayingScore--;
            if (MatchConfig.GetInstance().ShowTable)
                AddValues(team);
        }
    }

    private void ResetValues(Team team)
    {
        if (PlayingScore > team.PlayingScore)
        {
            Points -= 3;
            Won--;
            team.Lost--;
        }
        else if (PlayingScore == team.PlayingScore)
        {
            Points--;
            Drawn--;
            team.Points--;
            team.Drawn--;
        }
        else
        {
            Lost--;
            team.Won--;
            team.Points -= 3;
        }

        GoalsFor -= PlayingScore;
        GoalsAgainst -= team.PlayingScore;
        team.GoalsFor -= team.PlayingScore;
        team.GoalsAgainst -= PlayingScore;
    }

    private void AddValues(Team team)
    {
        if (PlayingScore > team.PlayingScore)
        {
            Points += 3;
            Won++;
            team.Lost++;
            if (StreakPlace <= 4 && StreakPlace >= 0)
            {
                Streak = Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "G");
                team.Streak = team.Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "P");
            }
        }
        else if (PlayingScore == team.PlayingScore)
        {
            Points++;
            Drawn++;
            team.Points++;
            team.Drawn++;
            if (StreakPlace <= 4 && StreakPlace >= 0)
            {
                Streak = Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "E");
                team.Streak = team.Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "E");
            }
        }
        else
        {
            Lost++;
            team.Points += 3;
            team.Won++;
            if (StreakPlace <= 4 && StreakPlace >= 0)
            {
                Streak = Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "P");
                team.Streak = team.Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "G");
            }
        }

        GoalsFor += PlayingScore;
        GoalsAgainst += team.PlayingScore;
        team.GoalsFor += team.PlayingScore;
        team.GoalsAgainst += PlayingScore;
    }

    // FAULTS CONTROLLER
    public void IncreaseFault()
    {
        PlayingFaults++;
    }

    public void ResetFaults()
    {
        PlayingFaults = 0;
    }

    public void StartMatch()
    {
        if (MatchConfig.GetInstance().ShowTable)
        {
            Drawn++;
            Points++;
            MatchesPlayed++;
            if (StreakPlace <= 4 && StreakPlace >= 0)
                Streak = Streak.Remove(StreakPlace, 1).Insert(StreakPlace, "E");
        }
    }

    // EXTRAS
    public int CompareTo(Team obj)
    {
        if (this.Points > obj.Points)
            return -1;
        if (this.Points < obj.Points)
            return 1;
        if (this.GoalsFor - this.GoalsAgainst > obj.GoalsFor - obj.GoalsAgainst)
            return -1;
        if (this.GoalsFor - this.GoalsAgainst < obj.GoalsFor - obj.GoalsAgainst)
            return 1;
        if (this.Won > obj.Won)
            return -1;
        if (this.Won < obj.Won)
            return 1;
        if (this.Lost < obj.Lost)
            return -1;
        if (this.Lost > obj.Lost)
            return 1;
        return 0;
    }

    public override string ToString()
    {
        return Name + ":\n" + "\tMatches Played:" + MatchesPlayed + "\n\tPoints:" + Points + "\n\tWon:" + Won +
               "\n\tDrawn:" + Drawn + "\n\tLost:" + Lost + "\n\tGoals For:" + GoalsFor + "\n\tGoals Against:" +
               GoalsAgainst + "\n\tStreak:" + Streak;
    }

    // GETTERS AND SETTERS
}