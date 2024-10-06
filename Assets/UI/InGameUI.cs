using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class SimpleRuntimeUI : MonoBehaviour
{
    private GameManager manager;
    private Label scoreLabel;
    private Label winOverlayscoreLabel;
    private Label timeLabel;
    private VisualElement endScreen;
    private VisualElement winScreen;
    private Button menuButton;
    private Button restartButton;
    private Button winMenuButton;
    private Button winRestartButton;
    private Button submitScoreButton;
    private TextField PlayerName;

    private LeaderboardListViewController leaderboardController;
    public VisualTreeAsset listEntryTemplate;

    private int finalScoreCached;
    private bool canSubmitScore = true;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        scoreLabel = uiDocument.rootVisualElement.Q<VisualElement>("GameplayLabels").Q<Label>("ScoreDisplay");
        timeLabel = uiDocument.rootVisualElement.Q<Label>("TimerDisplay");
        endScreen = uiDocument.rootVisualElement.Q("EndScreenOverlay");
        winScreen = uiDocument.rootVisualElement.Q("WinScreenOverlay");
        winOverlayscoreLabel = uiDocument.rootVisualElement.Q<Label>("WinOverlayScoreLabel");

        PlayerName = uiDocument.rootVisualElement.Q<TextField>("PlayerNameTextField");

        menuButton = uiDocument.rootVisualElement.Q<Button>("Menu");
        restartButton = uiDocument.rootVisualElement.Q<Button>("Restart");
        winMenuButton = uiDocument.rootVisualElement.Q<Button>("WinMenu");
        winRestartButton = uiDocument.rootVisualElement.Q<Button>("WinRestart");
        submitScoreButton = uiDocument.rootVisualElement.Q<Button>("SubmitScoreButton");

        restartButton.clicked += () =>
        {
            manager.RestartGame();
        };
        menuButton.clicked += () =>
        {
            manager.EnterMenu();
        };

        winRestartButton.clicked += () =>
        {
            Time.timeScale = 1;
            manager.RestartGame();
        };
        winMenuButton.clicked += () =>
        {
            Time.timeScale = 1;
            manager.EnterMenu();
        };

        submitScoreButton.clicked += () =>
        {
            if (canSubmitScore) 
            {
                LeaderboardManager.Instance.SubmitScoreToLeaderboard(PlayerName.text, finalScoreCached);
                
                canSubmitScore = false;
                submitScoreButton.SetEnabled(false);

                UpdateLeaderboard(LeaderboardManager.Instance.GetLeaderboardData());
            }
        };

        leaderboardController = new LeaderboardListViewController();
        
        HideAllOverlays();
        SetScore(0);
    }

    public void ToggleEndScreen(bool toggle)
    {
        endScreen.visible = toggle;
    }

    public void OpenWinOverlay(int inFinalScore, List<LeaderboardDataItem> inData)
    {
        finalScoreCached = inFinalScore;

        winScreen.visible = true;
        canSubmitScore = true;
        submitScoreButton.SetEnabled(true);

        leaderboardController.InitializeLeaderboardList(GetComponent<UIDocument>().rootVisualElement, listEntryTemplate);
        leaderboardController.UpdateLeaderboard(inData);
        winOverlayscoreLabel.text = inFinalScore.ToString();
    }

    public void HideAllOverlays()
    {
        endScreen.visible = false;
        winScreen.visible = false;
    }

    public void SetScore(int inScore)
    {
        scoreLabel.text = "Score: " + inScore.ToString();
    }
    public void SetRemainingTimeSeconds(float inTimeLeft)
    {
        timeLabel.text = inTimeLeft.ToString("F0") + "s";
    }
    public void SetRemainingTimeLow(bool inIsRemainingTimeLow)
    {
        timeLabel.EnableInClassList("warning", inIsRemainingTimeLow);
    }
    public void UpdateLeaderboard(List<LeaderboardDataItem> inData) 
    {
        if (leaderboardController != null) 
        {
            leaderboardController.UpdateLeaderboard(inData);
        }
    }
}