using UnityEngine;

public class StartScreen : UIPanel
{
    [SerializeField] private SessionManager _sessionManager;
    [SerializeField] private MoneyCounter _moneyCounter;

    private void OnEnable()
    {
        _moneyCounter.UpdateCounter();
    }

    public void Fight()
    {
        _sessionManager.StartSession();
        Disable();
    }
}
