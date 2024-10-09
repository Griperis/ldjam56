using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardListViewController
{
    VisualTreeAsset listEntryTemplate;
    ListView leaderboardListView;

    List<LeaderboardItem> data;

    public void InitializeLeaderboardList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        listEntryTemplate = listElementTemplate;
        leaderboardListView = root.Q<ListView>("LeaderboardListView");
        leaderboardListView.makeItem = () =>
        {
            var newListEntry = listEntryTemplate.Instantiate();
            var newListEntryLogic = new LeaderboardItemController();

            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        };

        leaderboardListView.bindItem = (item, index) =>
        {
            (item.userData as LeaderboardItemController)?.SetData(data[index]);
        };
    }

    public void UpdateLeaderboard(List<LeaderboardItem> inData)
    {
        data = inData;
        leaderboardListView.itemsSource = data;
        leaderboardListView.Rebuild();
    }
}
