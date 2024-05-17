using UnityEngine;

public class Test1 :MonoBehaviour
{
    [SerializeField] private Transform _parentForPopups;

    private void Start()
    {
        var queue = new TaskQueue();
        queue.InitLoadedTasks(_parentForPopups);
        queue.Enqueue(new DelayTask(1));
        queue.Enqueue(new PopupTask("Popup",new PopupModel(string.Empty,string.Empty,"Start","Just close popup"),_parentForPopups, queue));
        queue.Enqueue(new PopupTask("Popup", new PopupModel(string.Empty, "Ok", "Second popup", "You can press Ok"), _parentForPopups, queue,null,
            new System.Collections.Generic.List<ITask>() { new PopupTask("Popup", new PopupModel(string.Empty, string.Empty, "After second popup", "End"), _parentForPopups, queue)}));
        queue.Enqueue(new PopupTask("Popup", new PopupModel(string.Empty, string.Empty, "Third popup", "Just close popup"), _parentForPopups, queue));

    }
}