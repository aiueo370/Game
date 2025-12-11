using UnityEngine;

public class MoneyRepository : MonoBehaviour
{
    public double totalAssets;      // ‘‘Y
    public double maxHeldMoney;    // ‰ß‹‚ÌÅ‚‘‘YŠz

    // ‚¨‹à‚ğ‰ÁZ
    public void AddMoney(double _money)
    {
        totalAssets += _money;
    }

    // ‚¨‹à‚Ìg—p@‰Â”\‚È‚çtrue •s‰Â”\‚È‚çfalse‚ğ•Ô‚·
    public bool UseMoney(double _money)
    {
        //Debug.Log("Š‹à : " + totalAssets);
        if(totalAssets < _money)
        {
            return false;
        }
        else
        {
            totalAssets -= _money;
            return true;
        }
    }

    // Œ»İ‚Ì‘‘Y‚ğæ“¾
    public double GetTotalAssets() { return totalAssets; }

    // ‰ß‹Å‚‚Ì‘‘Y‚ğæ“¾
    public double GetMaxHeldMoney() { return maxHeldMoney; }

    private void Update()
    {
        if(totalAssets > maxHeldMoney) maxHeldMoney = totalAssets;
    }
}
