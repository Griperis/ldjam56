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

    public void SetData(LeaderboardDataItem inData)
    {
        playerNameLabel.text = inData.playerName;
        scoreLabel.text = inData.score.ToString();
    }
}
