using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : Singleton<DataManager>
{
    private string _path;

    public Data Data;

    public bool IsLoaded { get; private set; } = false;
    
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Load();
    }

    private void Load()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _path = Path.Combine(Application.persistentDataPath, "GameData.json");
#else
        _path = Path.Combine(Application.dataPath, "GameData.json");
#endif

        if (File.Exists(_path))
        {
            Data = JsonUtility.FromJson<Data>(File.ReadAllText(_path));
        }
        else
        {
            FirstPlay();
            Save();
        }

        IsLoaded = true;
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
    
    private void FirstPlay()
    {
        Data = new Data();
        StartCoroutine(SendEvent());
    }

    private IEnumerator SendEvent()
    {
        yield return new WaitForSeconds(.5f);
        
        FirebaseAnalytics.LogEvent("first_open");
    }
    
    public void Save()
    {
        File.WriteAllText(_path, JsonUtility.ToJson(Data));
    }
}

[Serializable]
public class Data
{
    public int Money;
    public List<int> PerksLevels;

    public Data()
    {
        Money = 0;
        PerksLevels = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
