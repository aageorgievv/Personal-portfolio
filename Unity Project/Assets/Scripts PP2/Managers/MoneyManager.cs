using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour, IManager
{
    public event Action<int> OnMoneyChanged;

    [SerializeField] bool infiniteMoney = false;
    [SerializeField] private int money = 500;

    public int GetMoney()
    {
        if (infiniteMoney)
        {
            return int.MaxValue;
        }
        return money;
    }

    public bool CanAfford(int amount)
    {
        if (infiniteMoney)
        {
            return true;
        }
        return money >= amount;
    }

    public void SpendMoney(int amount)
    {
        if (infiniteMoney)
        {
            OnMoneyChanged?.Invoke(int.MaxValue);
            return;
        }

        money -= amount;
        OnMoneyChanged?.Invoke(money);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }
}
