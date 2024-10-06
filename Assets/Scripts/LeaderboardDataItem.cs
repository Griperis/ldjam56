using System;
public class LeaderboardDataItem : IComparable
{
    public LeaderboardDataItem(string inName, int inScore) 
    {
        playerName = inName;
        score = inScore;
    }
    public int CompareTo(object obj)
    {
        var other = obj as LeaderboardDataItem;

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

    public string playerName;
    public int score;
}
