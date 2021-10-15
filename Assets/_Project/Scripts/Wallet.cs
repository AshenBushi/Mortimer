using System;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : Singleton<Wallet>
{
    public int Money { get; private set; }

    public event UnityAction OnMoneyChanged;

    private void Start()
    {
        SetMoney();
    }

    private void SetMoney()
    {
        Money = DataManager.Instance.Data.Money;
    }

    private void SaveMoney()
    {
        DataManager.Instance.Data.Money = Money;
        DataManager.Instance.Save();
    }

    public void AddMoney(int count)
    {
        Money += count;
        OnMoneyChanged?.Invoke();
        SaveMoney();
    }

    public bool TrySpendMoney(int price)
    {
        if (price <= Money)
        {
            Money -= price;
            OnMoneyChanged?.Invoke();
            SaveMoney();
            return true;
        }
        else
        {
            return false;
        }
    }
}
