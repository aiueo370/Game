using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public double money = 0;

    // お金を増やす
    public void AddMoney(double amount)
    {
        money += amount;
    }

    // 購入時にお金を使う（買えたらtrue、買えなければfalse）
    public bool UseMoney(double amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }
}