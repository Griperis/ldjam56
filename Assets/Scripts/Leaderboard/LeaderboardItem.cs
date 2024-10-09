using System;

public class LeaderboardItem : IComparable
{
    public string playerName;
    public int score;

    public LeaderboardItem(string inName, int inScore) 
    {
        playerName = inName;
        score = inScore;
    }

    public int CompareTo(object obj)
    {
        var other = obj as LeaderboardItem;

        if (this.score < other.score)
        {
            return 1;
        }
            
        if (this.score > other.score)
        {
            return -1;
        }

        return 0;
    }
}
