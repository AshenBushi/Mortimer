using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    
    private TMP_Text _text;
    private int _killCount = -1;

    public int KillCount => _killCount;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += UpdateCounter;
    }

    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= UpdateCounter;
    }

    private void Start()
    {
        UpdateCounter();
    }

    private void UpdateCounter(Enemy enemy = null)
    {
        _killCount++;
        _text.text = $"You killed: {_killCount}";
    }
}
