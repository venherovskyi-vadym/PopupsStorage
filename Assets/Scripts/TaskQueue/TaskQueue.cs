using Cysharp.Threading.Tasks;

public class TaskQueue
{
    private QueuePlayerPrefsJsonStorage<ITask> _storage = new QueuePlayerPrefsJsonStorage<ITask>();
    private ITask _currentTask;

    public void Enqueue(ITask task)
    {
        if (_currentTask == null)
        {
            _currentTask = task;
            _currentTask.Run();
            _currentTask.WaitComplete().ContinueWith(CheckAndRun);
            return;
        }

        _storage.Data.Enqueue(task);
    }

    private void CheckAndRun()
    {
        if (_storage.Data.Count == 0)
        {
            _currentTask = null;
            return;
        }

        _currentTask = _storage.Data.Dequeue();
        _currentTask.Run();
        _currentTask.WaitComplete().ContinueWith(CheckAndRun);
    }
}