using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardListViewController
{
    VisualTreeAsset listEntryTemplate;
    ListView leaderboardListView;

    List<string> testData;

    public void InitializeLeaderboardList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        listEntryTemplate = listElementTemplate;
        leaderboardListView = root.Q<ListView>("LeaderboardListView");

        testData = new List<string>();
        testData.Add("Hudry");
        testData.Add("Zbonek");
        testData.Add("Brisk");

        FillLeaderborad();
    }

    void FillLeaderborad()
    {
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
            (item.userData as LeaderboardItemController)?.SetData(testData[index]);
        };

        leaderboardListView.itemsSource = testData;
        leaderboardListView.Rebuild();
    }
}
