using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private GameManager manager;
    private Label scoreLabel;
    private VisualElement endScreen;


    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        scoreLabel = uiDocument.rootVisualElement.Q("ScoreDisplay") as Label;
        endScreen = uiDocument.rootVisualElement.Q("EndScreenOverlay");

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
}