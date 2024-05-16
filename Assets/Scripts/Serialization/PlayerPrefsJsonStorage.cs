using System.Collections.Generic;
using UnityEngine;


public class ListPlayerPrefsJsonStorage<T> : PlayerPrefsJsonStorage<List<T>>
{
    protected override string GetKey() => $"{Application.productName}.List<.{typeof(T).Name}>";
}

public class QueuePlayerPrefsJsonStorage<T> : PlayerPrefsJsonStorage<Queue<T>>
{
    protected override string GetKey() => $"{Application.productName}.Queue<.{typeof(T).Name}>";
}

public class PlayerPrefsJsonStorage<T> where T : class, new()
{
    private string _key;
    private T _data;
    public T Data => _data;
    protected virtual string GetKey() => $"{Application.productName}.{typeof(T).Name}";

    public PlayerPrefsJsonStorage()
    {
        _key = GetKey();
        _data = GetData();
    }

    private T GetData()
    {
        if (!PlayerPrefs.HasKey(_key))
        {
            return new T();
        }

        var json = PlayerPrefs.GetString(_key);
        T data = null;

        try
        {
            data = JsonUtility.FromJson<T>(json);
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
        PlayerPrefs.SetString(_key, JsonUtility.ToJson(_data));
    }
}