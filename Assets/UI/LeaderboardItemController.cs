using UnityEngine;
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

    public void SetData(LeaderboardItem inData)
    {
        playerNameLabel.text = $"{inData.rank + 1} {inData.playerName}";
        scoreLabel.text = inData.score.ToString();
        if (inData.isCurrentPlayer)
        {
            scoreLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        }
        ApplyColorModifiers(inData);
    }

    private void ApplyColorModifiers(LeaderboardItem inData)
    {
        // Applying styles in code is the thing (and in controler :D)!
        if (inData.rank == 0)
        {
            // Gold
            playerNameLabel.style.color = new StyleColor(new Color(1f, 215.0f / 255f, 0f));
        }
        else if (inData.rank == 1)
        {
            // Silver
            playerNameLabel.style.color = new StyleColor(new Color(192f / 255.0f, 192f / 255.0f, 192f / 255.0f));

        }
        else if (inData.rank == 2)
        {
            // Bronze
            playerNameLabel.style.color = new StyleColor(new Color(205 / 255f, 127f / 255f, 50f / 255f));
        }
    }
}
