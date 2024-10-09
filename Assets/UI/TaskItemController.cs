using UnityEngine.UIElements;

public class TaskItemController
{
    Label taskNameLabel;
    Label taskBullet;

    public void SetVisualElement(VisualElement visualElement)
    {
        taskNameLabel = visualElement.Q<Label>("TaskName");
        taskBullet = visualElement.Q<Label>("TaskBullet");
    }

    public void SetData(GameTask inData)
    {
        taskNameLabel.text = inData.Name;
        taskBullet.style.color = inData.color;
    }
}
