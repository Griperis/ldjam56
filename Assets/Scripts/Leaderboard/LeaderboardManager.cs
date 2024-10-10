using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;


public class LeaderboardMetadata
{
    public string submittedName = "???";
}

// Ideally this would have some inheritance as it combines local
// and online leaderboard systems.
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; } 
    public bool Dirty
    {
        get
        {
            return dirty;
        }
    }

    public bool UsingOnlineLeaderboard
    {
        get
        {
            return usingOnlineLeaderboard;
        }
    }

    public List<LeaderboardItem> LeaderboardData
    {
        get
        {
            return leaderboardData;
        }
    }

    private List<LeaderboardItem> leaderboardData = new List<LeaderboardItem>();

    private readonly string LeaderboardId = "highscore";
    
    private bool dirty = false;
    private bool usingOnlineLeaderboard = false;

    private async void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignedIn += () =>
            {
                usingOnlineLeaderboard = true;
            };

            AuthenticationService.Instance.SignInFailed += s =>
            {
                Debug.Log($" Failed to Authenticate player {s}");
                usingOnlineLeaderboard = false;
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async Task AddScoreOnline(int score, string name)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(
            LeaderboardId,
            score,
            new AddPlayerScoreOptions
            {
                Metadata = new Dictionary<string, string>() {
                    { "submittedName", name }
                },
            }
        );
        Debug.Log("Score added");
    }

    public async Task FetchScoresOnline()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { IncludeMetadata = true });

        leaderboardData.Clear();
        leaderboardData.Capacity = scoresResponse.Total;

        foreach (var score in scoresResponse.Results)
        {
            var metadata = GetLeaderboardMetadata(score);
            leaderboardData.Add(new LeaderboardItem(metadata.submittedName, (int)score.Score, score.Rank, score.PlayerId == AuthenticationService.Instance.PlayerId));    
        }
        dirty = true;
        Debug.Log("Score fetched");
    }

    public async void AddScoreAndFetchOnline(int score, string name)
    {
        await AddScoreOnline(score, name);
        await FetchScoresOnline();
    }

    public async Task<LeaderboardItem> GetPlayerScoreOnline()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        var metadata = GetLeaderboardMetadata(scoresResponse);
        return new LeaderboardItem(metadata.submittedName, (int)scoresResponse.Score, scoresResponse.Rank, true);
    }

    public async Task<List<LeaderboardItem>> FetchPlayerNearbyScoresOnline()
    {
        var playerScore = await GetPlayerScoreOnline();
        int offset = Mathf.Max(0, playerScore.rank - 5);
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = offset, Limit = 10, IncludeMetadata = true });

        var firstScoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Limit = 3, IncludeMetadata = true });
        
        leaderboardData.Clear();


        List<string> firstPlayerIds = new List<string>();

        // Populate first three entries
        foreach (var score in firstScoresResponse.Results)
        {
            var metadata = GetLeaderboardMetadata(score);
            leaderboardData.Add(new LeaderboardItem(metadata.submittedName, (int)score.Score, score.Rank, score.PlayerId == AuthenticationService.Instance.PlayerId));
            firstPlayerIds.Add(score.PlayerId);
        }

        // Populate additional entries, remove if in first three
        foreach (var score in scoresResponse.Results)
        {
            var metadata = GetLeaderboardMetadata(score);
            if (!firstPlayerIds.Contains(score.PlayerId))
            {
                leaderboardData.Add(new LeaderboardItem(metadata.submittedName, (int)score.Score, score.Rank, score.PlayerId == AuthenticationService.Instance.PlayerId));
            }
        }

        dirty = true;
        return leaderboardData;
    }

    public async void AddScoreAndFetchNearbyPlayersOnline(int score, string name)
    {
        await AddScoreOnline(score, name);
        await FetchPlayerNearbyScoresOnline();
    }

    public bool SaveLocalHighscore(int score)
    {
        int previousScore = PlayerPrefs.GetInt(LeaderboardId, 0);
        bool isHigher = score > previousScore;
        if (isHigher)
        {
            PlayerPrefs.SetInt(LeaderboardId, score);
        }

        return isHigher;
    }

    public int GetLatestLocalHighscore()
    {
        return PlayerPrefs.GetInt(LeaderboardId, 0);
    }

    public void ResetDirty()
    {
        dirty = false;
    }

    private LeaderboardMetadata GetLeaderboardMetadata(LeaderboardEntry leaderboardEntry)
    {
        var metadata = JsonUtility.FromJson<LeaderboardMetadata>(leaderboardEntry.Metadata);
        if (metadata == null)
        {
            return new LeaderboardMetadata();
        }
        return metadata;
    }
}
