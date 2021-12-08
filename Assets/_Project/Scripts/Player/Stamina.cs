using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    [SerializeField] private int _maxStaminaCount;
    [SerializeField] private int _regeneratePerSecond;

    private PlayerStateMachine _playerStateMachine;
    private float _timer = 0;
    private float _staminaCount;
    private bool _canRegenerate = true;

    public int MaxStaminaCount => _maxStaminaCount;
    public float StaminaCount => _staminaCount;
    public event UnityAction OnStaminaChanged;

    private void Awake()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    public void Init()
    {
        _maxStaminaCount += (int)PerksHandler.Instance.GetPerkBoost(PerkName.Stamina);
        _staminaCount = _maxStaminaCount;
    }
    
    private void Update()
    {
        if (!_canRegenerate || _staminaCount == _maxStaminaCount) return;
        
        _timer += Time.deltaTime;

        if (_timer < 1) return;
        
        if(_playerStateMachine.IsBlocking)
            SpendStamina(1);
        else
            RegenerateStamina();
        
        _timer = 0;
    }

    private void RegenerateStamina()
    {
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
    
    public void SpendStamina(float value)
    {
        _staminaCount -= value;

        if (_staminaCount <= 0)
        {
            _staminaCount = 0;
            _playerStateMachine.ToIdle();
            StartCoroutine(StopRegenerate());
        }
        
        OnStaminaChanged?.Invoke();
    }
}
