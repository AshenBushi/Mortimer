using TMPro;
using UnityEngine;

public class WaveBar : Bar
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TMP_Text _waveNumber;
    
    private void OnEnable()
    {
        _enemySpawner.OnEnemySpawned += UpdateBar;
    }

    private void OnDisable()
    {
        _enemySpawner.OnEnemySpawned -= UpdateBar;
    }

    private void UpdateBar(int enemiesSpawned, int currentWave)
    {
        if(_waveNumber.text != currentWave.ToString())
            SetBarValue(20, enemiesSpawned);
        else
            ChangeBarValue(enemiesSpawned);
        
        _waveNumber.text = currentWave.ToString();
    }
}
