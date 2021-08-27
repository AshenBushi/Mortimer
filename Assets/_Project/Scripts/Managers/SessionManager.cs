using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameScreen _gameScreen;
    
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
    }
}
