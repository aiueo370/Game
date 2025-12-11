using UnityEngine;

[CreateAssetMenu(fileName = "GlobalProdFixedBuffItem", menuName = "Clicker/Item/Production/Global/Fixed Value")]
public class GlobalProdFixedBuffItem : ItemData
{
    [Tooltip("ó·: 0.15 Å® +15%Åi= Å~1.15Åj")]
    public double percent = 0.15;

    public override void ApplyEffect(ItemManager manager)
    {
        manager.productionBuffRegistry.AddGlobalFixedMultiplier(1.0 + percent);
    }
}