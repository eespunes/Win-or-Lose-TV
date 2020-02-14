using System;

public class Team : IComparable
{
    private int points;
    private int goalsFor;
    private int goalsAgainst;
    private int won, drawn, lost;
    private bool isPlaying;
    private char homeVisitor;
    private int playingScore;

    public Team(int points, int goalsFor, int goalsAgainst, int won, int drawn, int lost)
    {
        this.points = points;
        this.goalsFor = goalsFor;
        this.goalsAgainst = goalsAgainst;
        this.won = won;
        this.drawn = drawn;
        this.lost = lost;
        isPlaying = false;
    }

    public Team(int points, int goalsFor, int goalsAgainst, int won, int drawn, int lost, bool isPlaying,
        char homeVisitor)
    {
        this.points = points;
        this.goalsFor = goalsFor;
        this.goalsAgainst = goalsAgainst;
        this.won = won;
        this.drawn = drawn;
        this.lost = lost;
        this.isPlaying = isPlaying;
        this.homeVisitor = homeVisitor;
    }


    //Getters and Setters
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

    public bool IsPlaying
    {
        get => isPlaying;
    }

    public char HomeVisitor
    {
        get => homeVisitor;
    }

    public int PlayingScore
    {
        get => playingScore;
        set => playingScore = value;
    }

    public int CompareTo(object obj)
    {
        if (obj is Team)
        {
            Team other = (Team) obj;

            if (this.points > other.Points)
                return 1;
            if (this.points < other.Points)
                return -1;
            if (this.goalsFor - this.goalsAgainst > other.GoalsFor - other.goalsAgainst)
                return 1;
            if (this.goalsFor - this.goalsAgainst < other.GoalsFor - other.goalsAgainst)
                return -1;
            if (this.won > other.Won)
                return 1;
            if (this.won < other.Won)
                return -1;
            if (this.lost < other.Lost)
                return 1;
            if (this.lost > other.Lost)
                return -1;
            return 0;
        }

        return -1;
    }
}