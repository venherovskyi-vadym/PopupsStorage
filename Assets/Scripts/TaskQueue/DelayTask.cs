using Cysharp.Threading.Tasks;
using System;

[Serializable]
public class DelayTask : ITask, ITaskFinish
{
    public string Name { get; }
    private readonly float _delay;
    [NonSerialized] private UniTaskCompletionSource _completionSource;

    public DelayTask(float delay)
    {
        _delay = delay;
    }

    public async void Run()
    {
        _completionSource = new UniTaskCompletionSource();
        await UniTask.Delay(TimeSpan.FromSeconds(_delay));
        CompleteTask();
    }

    public UniTask WaitComplete()
    {
        return _completionSource.Task;
    }

    public void CompleteTask()
    {
        _completionSource.TrySetResult();
    }
}