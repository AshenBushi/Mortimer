using System;
using Firebase.Analytics;
using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private GameObject _moneyCounter;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private GetCoins _getCoins;

    private void Start()
    {
        AudioManager.Instance.PlayMenuMusic();
    }

    public void StartSession()
    {
        _getCoins.gameObject.SetActive(false);
        _gameScreen.Show(AnimationName.Instantly);
        _enemySpawner.StartSession();
        _player.Init();
        SkillsHandler.Instance.Init();
        AudioManager.Instance.PlayBattleMusic();
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
    }

    public void EndSession()
    {
        _gameScreen.Hide(AnimationName.Instantly);
        _enemySpawner.EndSession();
        _moneyCounter.SetActive(false);
        _loseScreen.Show(AnimationName.Slowly);
        AudioManager.Instance.PlayMenuMusic();
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
        FirebaseAnalytics.LogEvent($"waves_complete_{_enemySpawner.CurrentWave}");
    }
}
