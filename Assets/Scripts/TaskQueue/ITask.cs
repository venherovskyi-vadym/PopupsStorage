using Cysharp.Threading.Tasks;

public interface ITask
{
    string Name { get; }
    UniTask WaitComplete();
    void Run();
}