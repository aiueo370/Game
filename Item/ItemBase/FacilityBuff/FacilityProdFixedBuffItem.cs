using UnityEngine;

[CreateAssetMenu(fileName = "FacilityProdFixedBuffItem", menuName = "Clicker/Item/Production/Facility/Fixed Value")]
public class FacilityProdFixedBuffItem : ItemData
{
    public FacilityName targetFacility;
    [Tooltip("ó·: 0.20 Å® +20%Åi= Å~1.2Åj")]
    public double percent = 0.20;

    public override void ApplyEffect(ItemManager manager)
    {
        manager.productionBuffRegistry.AddFacilityFixedMultiplier(targetFacility, 1.0 + percent);
    }
}