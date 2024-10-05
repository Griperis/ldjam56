using UnityEngine;
using UnityEngine.UIElements;

public class SimpleRuntimeUI : MonoBehaviour
{
    private Label scoreLabel;

    //Add logic that interacts with the UI controls in the `OnEnable` methods
    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        scoreLabel = uiDocument.rootVisualElement.Q("ScoreDisplay") as Label;

        SetScore(0);
    }

    private void OnDisable()
    {
    }

    public void SetScore(int inScore)
    {
        scoreLabel.text = "Score: " + inScore.ToString();
    }
}