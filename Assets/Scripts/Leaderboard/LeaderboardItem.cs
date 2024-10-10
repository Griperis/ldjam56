using System;

public class LeaderboardItem : IComparable
{
    public string playerName;
    public int score;
    public int rank;

    public bool isCurrentPlayer = false;

    public LeaderboardItem(string inName, int inScore, int inRank, bool inIsCurrentPlayer = false) 
    {
        playerName = inName;
        score = inScore;
        rank = inRank;
        isCurrentPlayer = inIsCurrentPlayer;
    }

    public int CompareTo(object obj)
    {
        var other = obj as LeaderboardItem;

        if (rank < other.rank)
        {
            return 1;
        }
            
        if (rank > other.rank)
        {
            return -1;
        }

        return 0;
    }
}
