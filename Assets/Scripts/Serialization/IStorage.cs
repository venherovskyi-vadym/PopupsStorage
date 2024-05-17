public interface IStorage<T>
{
    bool HasItem();
    void Enqueue(T item);
    void InsertInFront(T item);
    T Dequeue();
    void Save();
}