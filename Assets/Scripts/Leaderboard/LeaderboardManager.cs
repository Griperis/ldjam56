using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UIElements;


public class LeaderboardMetadataJSON
{
    public string submittedName;
}

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
                Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
                usingOnlineLeaderboard = true;
            };

            AuthenticationService.Instance.SignInFailed += s =>
            {
                // Take some action here...
                Debug.Log($" Failed to Authenticate player {s}");
                usingOnlineLeaderboard = false;
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async Task AddScore(int score, string name)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(
            LeaderboardId,
            score,
            new AddPlayerScoreOptions
            {
                Metadata = new Dictionary<string, string>() {
                    { "submittedName", name }
                }
            }
        );
        Debug.Log("Score added");

    }

    public async Task FetchScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { IncludeMetadata = true });

        leaderboardData.Clear();
        leaderboardData.Capacity = scoresResponse.Total;

        foreach (var score in scoresResponse.Results)
        {
            var metadataDict = JsonUtility.FromJson<LeaderboardMetadataJSON>(score.Metadata);
            string name = "???";
            if (metadataDict != null)
            {
                name = metadataDict.submittedName;
            }
            leaderboardData.Add(new LeaderboardItem(name, (int)score.Score));    
        }
        Debug.Log("Score fetched");
    }

    public async void AddScoreAndFetch(int score, string name)
    {
        await AddScore(score, name);
        await FetchScores();
        dirty = true;
    }
    public void ResetDirty()
    {
        dirty = false;
    }
}
