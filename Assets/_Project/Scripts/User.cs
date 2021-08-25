
using System;
using UnityEngine.Events;

public class User : Singleton<User>
{
    public int Money { get; private set; }

    public event UnityAction OnMoneyChanged;

    private void Start()
    {
        SetMoney();
    }

    private void OnDisable()
    {
        DataManager.Instance.Data.Money = Money;
        DataManager.Instance.Save();
    }

    private void SetMoney()
    {
        Money = DataManager.Instance.Data.Money;
        OnMoneyChanged?.Invoke();
    }
    
    public void SpendMoney(int value)
    {
        Money -= value;
        OnMoneyChanged?.Invoke();
    }

    public void AddMoney(int value)
    {
        Money += value;
        OnMoneyChanged?.Invoke();
    }
}
