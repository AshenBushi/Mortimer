using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TopPanel _topPanel;
    
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
        _topPanel.Enable();
        _enemySpawner.StartSession();
        _player.Init();
    }

    public void EndSession()
    {
        _topPanel.Disable();
        _enemySpawner.EndSession();
    }
}
