using UnityEngine;

[CreateAssetMenu(fileName = "FacilityProdFromCountBuffItem", menuName = "Clicker/Item/Production/Facility/Facility Value")]
public class FacilityProdFromCountBuffItem : ItemData
{
    public FacilityName targetFacility; // 強化される側
    public FacilityName sourceFacility; // 参照する側（所持数）
    [Tooltip("1つあたりの倍率加算量（0.02 → 10個で +0.2 → ×1.2）")]
    public double perUnitAddend = 0.02;

    public override void ApplyEffect(ItemManager manager)
    {
        manager.productionBuffRegistry.AddFacilityPerUnitAddend(targetFacility, sourceFacility, perUnitAddend);
    }
}