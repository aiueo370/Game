using UnityEngine;

[CreateAssetMenu(fileName = "GlobalProdFromCountBuffItem", menuName = "Clicker/Item/Production/Global/Facility Value")]
public class GlobalProdFromCountBuffItem : ItemData
{
    public FacilityName sourceFacility;
    [Tooltip("1Ç¬Ç†ÇΩÇËÇÃî{ó¶â¡éZó Åi0.01 Å® 50å¬Ç≈ +0.5 Å® Å~1.5Åj")]
    public double perUnitAddend = 0.01;

    public override void ApplyEffect(ItemManager manager)
    {
        manager.productionBuffRegistry.AddGlobalPerUnitAddend(sourceFacility, perUnitAddend);
    }
}
