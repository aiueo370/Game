using UnityEngine;

[CreateAssetMenu(fileName = "ClickMultiplierFromCountItem", menuName = "Clicker/Item/Click/Multiplier/FacilityValue")]
public class ClickMultiplierFromCountItem : ItemData
{
    public FacilityName sourceFacility;
    [Tooltip("1‚Â‚ ‚½‚è‚Ì”{—¦‰ÁZ—ÊB—á: 0.02 ¨ 10ŒÂ‚Å (1 + 0.2) = 1.2”{")]
    public double perUnitAddend = 0.02;

    public override void ApplyEffect(ItemManager manager)
    {
        int c = manager.GetFacilityCount(sourceFacility);
        double factor = 1.0 + perUnitAddend * c;
        manager.moneyClickManager.MultiplyClick(factor);
    }
}
