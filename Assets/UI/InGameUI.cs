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
    private VisualElement leaderboardScreen;
    private VisualElement pauseScreen;
    private Button menuButton;
    private Button restartButton;
    private Button leaderboardMenuButton;
    private Button leaderboardRestartButton;
    private Button submitScoreButton;
    private Button pauseMenuButton;
    private Button pauseContinueButton;

    private ProgressBar chargeProgress;

    private TextField PlayerName;

    private LeaderboardListViewController leaderboardController;
    public VisualTreeAsset listEntryTemplate;
    public ButtonAudio buttonAudio;
    private TaskListViewController taskViewController;

    public VisualTreeAsset leaderboardListEntryTemplate;
    public VisualTreeAsset taskListEntryTemplate;

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
        winOverlayscoreLabel = uiDocument.rootVisualElement.Q<Label>("WinOverlayScoreLabel");

        endScreen = uiDocument.rootVisualElement.Q("EndScreenOverlay");
        winScreen = uiDocument.rootVisualElement.Q("WinScreenOverlay");
        leaderboardScreen = uiDocument.rootVisualElement.Q("LeaderboardScreenOverlay");
        pauseScreen = uiDocument.rootVisualElement.Q("PauseScreenOverlay");

        PlayerName = uiDocument.rootVisualElement.Q<TextField>("PlayerNameTextField");

        menuButton = uiDocument.rootVisualElement.Q<Button>("Menu");
        restartButton = uiDocument.rootVisualElement.Q<Button>("Restart");
        leaderboardMenuButton = uiDocument.rootVisualElement.Q<Button>("WinMenu");
        leaderboardRestartButton = uiDocument.rootVisualElement.Q<Button>("WinRestart");
        submitScoreButton = uiDocument.rootVisualElement.Q<Button>("SubmitScoreButton");
        pauseMenuButton = uiDocument.rootVisualElement.Q<Button>("PauseMenu");
        pauseContinueButton = uiDocument.rootVisualElement.Q<Button>("PauseContinue");
        chargeProgress = uiDocument.rootVisualElement.Q<ProgressBar>("Charge");
        chargeProgress.visible = false;

        buttonAudio.AddButtonSounds(uiDocument);

        restartButton.clicked += () =>
        {
            manager.RestartGame();
        };
        menuButton.clicked += () =>
        {
            manager.EnterMenu();
        };

        leaderboardRestartButton.clicked += () =>
        {
            Time.timeScale = 1;
            manager.RestartGame();
        };
        leaderboardMenuButton.clicked += () =>
        {
            Time.timeScale = 1;
            manager.EnterMenu();
        };

        pauseMenuButton.clicked += () =>
        {
            TogglePauseScreenInternal(false);
            manager.EnterMenu();
        };
        pauseContinueButton.clicked += () =>
        {
            TogglePauseScreenInternal(false);
        };

        submitScoreButton.clicked += () =>
        {
            if (canSubmitScore) 
            {
                LeaderboardManager.Instance.SubmitScoreToLeaderboard(PlayerName.text, finalScoreCached);
                
                canSubmitScore = false;
                submitScoreButton.SetEnabled(false);
                winScreen.visible = false;
                leaderboardScreen.visible = true;

                UpdateLeaderboard(LeaderboardManager.Instance.GetLeaderboardData());
            }
        };

        leaderboardController = new LeaderboardListViewController();
        taskViewController = new TaskListViewController();

        taskViewController.InitializeTaskList(GetComponent<UIDocument>().rootVisualElement, taskListEntryTemplate);

        HideAllOverlays();
        SetScore(0);
    }

    public void ToggleEndScreen(bool toggle)
    {
        endScreen.visible = toggle;
    }

    public void TogglePauseScreen()
    {
        if (pauseScreen.visible == true)
        {
            TogglePauseScreenInternal(false);
        }
        else 
        {
            TogglePauseScreenInternal(true);
        }
    }

    private void TogglePauseScreenInternal(bool toggle)
    {
        pauseScreen.visible = toggle;
        Time.timeScale = toggle ? 0.0f : 1.0f;
    }

    public void OpenWinOverlay(int inFinalScore, List<LeaderboardDataItem> inData)
    {
        finalScoreCached = inFinalScore;

        winScreen.visible = true;
        canSubmitScore = true;
        submitScoreButton.SetEnabled(true);

        leaderboardController.InitializeLeaderboardList(GetComponent<UIDocument>().rootVisualElement, leaderboardListEntryTemplate);
        leaderboardController.UpdateLeaderboard(inData);
        winOverlayscoreLabel.text = inFinalScore.ToString();
    }

    public void HideAllOverlays()
    {
        endScreen.visible = false;
        winScreen.visible = false;
        pauseScreen.visible = false;
        leaderboardScreen.visible = false;
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
    public void UpdateTasks(List<Task> inData)
    {
        if (taskViewController != null)
        {
            taskViewController.UpdateTasks(inData);
        }
    }

    public void SetChargeProgress(float inProgress)
    {
        if (inProgress > 0)
        {
            chargeProgress.visible = true;
        }
        chargeProgress.value = inProgress;
    }

    public void DisableChargeProgress()
    {
        chargeProgress.visible = false;
    }


}