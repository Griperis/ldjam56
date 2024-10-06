using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskListViewController
{
    VisualTreeAsset listEntryTemplate;
    ListView taskListView;

    List<Task> data;

    public void InitializeTaskList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        listEntryTemplate = listElementTemplate;
        taskListView = root.Q<ListView>("TaskListView");
    }

    public void UpdateTasks(List<Task> inData)
    {
        data = inData;

        taskListView.makeItem = () =>
        {
            var newListEntry = listEntryTemplate.Instantiate();
            var newListEntryLogic = new TaskItemController();

            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        };

        taskListView.bindItem = (item, index) =>
        {
            (item.userData as TaskItemController)?.SetData(data[index]);
        };

        taskListView.itemsSource = data;
        taskListView.Rebuild();
    }
}
