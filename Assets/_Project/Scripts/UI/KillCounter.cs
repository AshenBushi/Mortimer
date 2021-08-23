using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    
    private TMP_Text _text;
    private int _killCount = -1;

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
        UpdateCounter(new Enemy());
    }

    private void UpdateCounter(Enemy enemy)
    {
        _killCount++;
        _text.text = _killCount.ToString();
    }
}
