using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    [SerializeField] private int _maxStaminaCount;
    [SerializeField] private int _defaultRegeneratePerSecond;

    private PlayerStateHandler _playerStateHandler;
    private float _timer = 0;
    private int _staminaCount;
    private int _regeneratePerSecond;
    private bool _canRegenerate = true;

    private bool IsPlayerBlocking => _playerStateHandler.IsBlocking;

    public int MaxStaminaCount => _maxStaminaCount;
    public int StaminaCount => _staminaCount;
    
    public event UnityAction OnStaminaChanged;

    private void Awake()
    {
        _playerStateHandler = GetComponent<PlayerStateHandler>();
        
        Init();
    }

    private void Init()
    {
        _staminaCount = _maxStaminaCount;
        _regeneratePerSecond = _defaultRegeneratePerSecond;
    }
    
    private void Update()
    {
        if (!_canRegenerate || _staminaCount == _maxStaminaCount) return;
        _timer += Time.deltaTime;

        if (_timer < 1) return;
        RegenerateStamina();
        _timer = 0;
    }

    private void RegenerateStamina()
    {
        _regeneratePerSecond = IsPlayerBlocking ? _defaultRegeneratePerSecond / 2 : _defaultRegeneratePerSecond;

        if (_staminaCount + _regeneratePerSecond > _maxStaminaCount)
            _staminaCount = _maxStaminaCount;
        else
            _staminaCount += _regeneratePerSecond;
        
        OnStaminaChanged?.Invoke();
    }

    private IEnumerator StopRegenerate()
    {
        _canRegenerate = false;

        yield return new WaitForSeconds(2f);

        _canRegenerate = true;
    }
    
    public void SpendStamina(int value)
    {
        _staminaCount -= value;

        if (_staminaCount <= 0)
        {
            _staminaCount = 0;
            _playerStateHandler.ToIdle();
            StartCoroutine(StopRegenerate());
        }
        
        OnStaminaChanged?.Invoke();
    }
}
