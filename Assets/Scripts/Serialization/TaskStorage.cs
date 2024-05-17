public class TaskStorage<T> : IStorage<T>
{
    private ListPlayerPrefsJsonStorage<T> _underLyingStorage = new ListPlayerPrefsJsonStorage<T>();

    public void Enqueue(T item)
    {
        _underLyingStorage.Data.Add(item);
    }

    public void InsertInFront(T item)
    {
        _underLyingStorage.Data.Insert(0, item);
    }

    public T Dequeue()
    {
        if (!HasItem())
        {
            return default(T);
        }

        var item = _underLyingStorage.Data[0];
        _underLyingStorage.Data.RemoveAt(0);
        return item;
    }

    public void Save()
    {
        _underLyingStorage.Save();
    }

    public bool HasItem()
    {
        return _underLyingStorage.Data.Count > 0;
    }
}