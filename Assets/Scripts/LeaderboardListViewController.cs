using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardListViewController
{
    VisualTreeAsset listEntryTemplate;
    ListView leaderboardListView;

    List<LeaderboardDataItem> data;

    public void InitializeLeaderboardList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        listEntryTemplate = listElementTemplate;
        leaderboardListView = root.Q<ListView>("LeaderboardListView");
    }

    public void UpdateLeaderboard(List<LeaderboardDataItem> inData)
    {
        data = inData;

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

        leaderboardListView.itemsSource = data;
        leaderboardListView.Rebuild();
    }
}
