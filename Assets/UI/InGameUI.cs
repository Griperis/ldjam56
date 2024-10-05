using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private GameManager manager;
    private Label scoreLabel;
    private Label timeLabel;
    private VisualElement endScreen;
    private Button menuButton;
    private Button restartButton;


    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreDisplay");
        timeLabel = uiDocument.rootVisualElement.Q<Label>("TimerDisplay");
        endScreen = uiDocument.rootVisualElement.Q("EndScreenOverlay");

        menuButton = uiDocument.rootVisualElement.Q<Button>("Menu");
        restartButton = uiDocument.rootVisualElement.Q<Button>("Restart");

        restartButton.clicked += () =>
        {
            manager.RestartGame();
        };
        menuButton.clicked += () =>
        {
            manager.EnterMenu();
        };

        ToggleEndScreen(false);
        SetScore(0);
    }

    private void OnDisable()
    {
    }

    public void ToggleEndScreen(bool toggle)
    {
        endScreen.visible = toggle;
     }

    public void SetScore(int inScore)
    {
        scoreLabel.text = "Score: " + inScore.ToString();
    }
    public void SetRemainingTimeSeconds(float inTimeLeft)
    {
        timeLabel.text = "Time left: " + inTimeLeft.ToString("F0") + "s";
    }
}