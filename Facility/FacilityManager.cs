using UnityEngine;
using System.Collections.Generic;

// {İ‚ğ‚Ü‚Æ‚ß‚ÄŠÇ—‚·‚é‚â‚Â
public class FacilityManager : MonoBehaviour
{
    // ‚¨‹àŠÇ—ƒNƒ‰ƒX
    [SerializeField] private MoneyRepository moneyRepository;
    [SerializeField] private MoneyIdleManager moneyIdleManager;

    // {İ‚ÌƒŠƒXƒg
    public List<Facility> facilities = new List<Facility>();

    // {İw“üˆ—
    public bool BuyFacility(int index)
    {
        //Debug.Log("¤ w“üŠJn");
        // ”ÍˆÍŠO‚È‚ç”ƒ‚¦‚È‚¢
        if (index < 0 || index >= facilities.Count) return false;

        Facility facility = facilities[index];
        double price = facility.currentPrice;

        //Debug.Log("ƒAƒCƒeƒ€‚Ì’l’i" + price); 

        // ‚¨‹à‘«‚è‚Ä‚½‚çw“ü
        if (moneyRepository.UseMoney(price))
        {
            //Debug.Log("Z w“üŠ®—¹");
            facility.count++; // Š”ƒAƒbƒv
            moneyIdleManager.ChangeMoneyPerSecond();
            return true;
        }

        Debug.Log("~ w“ü¸”s");
        // ”ƒ‚¦‚È‚©‚Á‚½
        return false;
    }

    // ƒx[ƒX‰¿Ši‚ğæ“¾
    public double GetBasePrice(int _index)
    {
        return facilities[_index].data.basePrice;
    }

    public double GetCurrentPrice(int _index)
    {
        return facilities[_index].currentPrice;
    }
}