using Cysharp.Threading.Tasks;

public class Task : ITask, ITaskFinish
{
    public string Name { get; }

    private UniTaskCompletionSource _completionSource;

    public void Run()
    {
        _completionSource = new UniTaskCompletionSource();
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