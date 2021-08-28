using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private GameObject _moneyCounter;
    [SerializeField] private LoseScreen _loseScreen;

    public void StartSession()
    {
        _gameScreen.Enable();
        _enemySpawner.StartSession();
        _player.Init();
        SkillsHandler.Instance.Init();
    }

    public void EndSession()
    {
        _gameScreen.Disable();
        _enemySpawner.EndSession();
        _moneyCounter.SetActive(false);
        _loseScreen.Enable();
    }
}
