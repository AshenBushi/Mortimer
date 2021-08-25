using System;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : Singleton<DataManager>
{
    public Data Data;

    protected override void Awake()
    {
        base.Awake();

        Load();
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        Save();
    }
#endif
    private void OnApplicationQuit()
    {
        Save();
    }
    
    private void Load()
    {
        Data = PlayerPrefs.HasKey("Data") ? JsonUtility.FromJson<Data>(PlayerPrefs.GetString("Data")) : new Data();
    }

    public void Save()
    {
        PlayerPrefs.SetString("Data", JsonUtility.ToJson(Data));
    }
}

[Serializable]
public class Data
{
    public int Money;
}