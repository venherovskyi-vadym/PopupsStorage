using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class ListBinaryStorage<T> : BinaryStorage<List<T>>
{
    protected override string GetKey() => $"{Application.productName}.List.{typeof(T).Name}.bin";
}

public class QueueBinaryStorage<T> : BinaryStorage<Queue<T>>
{
    protected override string GetKey() => $"{Application.productName}.Queue.{typeof(T).Name}.bin";
}

public class BinaryStorage<T> where T : class, new()
{
    private string _key;
    private T _data;
    public T Data => _data;
    protected virtual string GetKey() => $"{Application.productName}.{typeof(T).Name}.bin";

    public BinaryStorage()
    {
        _key = GetKey();
        _data = GetData();
    }

    private T GetData()
    {
        var path = Path.Combine(Application.persistentDataPath, GetKey());
        if (!Directory.Exists(Application.persistentDataPath) || !File.Exists(path))
        {
            return new T();
        }

        T data;

        try
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                var obj = formatter.Deserialize(stream);
                data = obj as T;
            }
        }
        catch
        {
            return new T();
        }

        if (data != null)
        {
            return data;
        }

        return new T();
    }

    public void Save()
    {
        var path = Path.Combine(Application.persistentDataPath, GetKey());

        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        using (var stream = new FileStream(path, FileMode.OpenOrCreate))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, _data);
        }
    }
}