using UnityEngine;

[CreateAssetMenu(fileName = "ClickMultiplierFixedItem", menuName = "Clicker/Item/Click/Multiplier/Fixed Value")]
public class ClickMultiplierFixedItem : ItemData
{
    [Tooltip("—á: 1.5 = 1.5”{, 2.0 = 2”{")]
    public double factor = 1.5;

    public override void ApplyEffect(ItemManager manager)
    {
        manager.moneyClickManager.MultiplyClick(factor);
    }
}