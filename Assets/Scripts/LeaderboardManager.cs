using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{

    public static LeaderboardManager Instance { get; private set; }

    private List<LeaderboardDataItem> leaderboardData = new List<LeaderboardDataItem>();
    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<LeaderboardDataItem> GetLeaderboardData()
    {
        return leaderboardData;
    }

    public void SubmitScoreToLeaderboard(string inPlayerName, int inFinalScore)
    {
        LeaderboardDataItem item = new LeaderboardDataItem(inPlayerName, inFinalScore);
        leaderboardData.Add(item);
        leaderboardData.Sort();
    }
}
