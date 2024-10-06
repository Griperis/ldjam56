using UnityEngine.UIElements;

public class LeaderboardItemController
{
    Label playerNameLabel;
    Label scoreLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        playerNameLabel = visualElement.Q<Label>("PlayerNameLabel");
        scoreLabel = visualElement.Q<Label>("ScoreLabel");
    }

    public void SetData(string inPlayerName)
    {
        playerNameLabel.text = inPlayerName;
        //scoreLabel.text = inScore.ToString();
    }
}
