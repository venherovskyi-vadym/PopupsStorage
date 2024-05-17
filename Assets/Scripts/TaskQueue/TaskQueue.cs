using Cysharp.Threading.Tasks;
using UnityEngine;

public class TaskQueue
{
    private IStorage<ITask> _storage = new TaskStorage<ITask>();
    private ITask _currentTask;

    public void InitLoadedTasks(Transform parent)
    {
        var data = _storage.GetData();
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] is PopupTask popupTask)
            {
                popupTask.InitLoadedTask(parent, this);
            }
        }
    }

    public void Enqueue(ITask task)
    {
        if (_currentTask == null)
        {
            _currentTask = task;
            _currentTask.Run();
            _currentTask.WaitComplete().ContinueWith(CheckAndRun);
            return;
        }

        _storage.Enqueue(task);
        _storage.Save();
    }

    public void InsertInFront(ITask task)
    {
        _storage.InsertInFront(task);
    }

    private void CheckAndRun()
    {
        if (!_storage.HasItem())
        {
            _currentTask = null;
            return;
        }

        _currentTask = _storage.Dequeue();
        _storage.Save();
        _currentTask.Run();
        _currentTask.WaitComplete().ContinueWith(CheckAndRun);
    }
}