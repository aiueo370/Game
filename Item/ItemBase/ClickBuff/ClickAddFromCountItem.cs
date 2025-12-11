using UnityEngine;

[CreateAssetMenu(fileName = "ClickAddFromCountItem", menuName = "Clicker/Item/Click/Add/Facility Value")]
public class ClickAddFromCountItem : ItemData
{
    public FacilityName sourceFacility;
    [Tooltip("1‚Â‚ ‚½‚è‚Ì‰ÁŽZ’l")]
    public double addendPerUnit = 1.0;

    public override void ApplyEffect(ItemManager manager)
    {
        int c = manager.GetFacilityCount(sourceFacility);
        manager.moneyClickManager.AddClickAddend(addendPerUnit * c);
    }
}