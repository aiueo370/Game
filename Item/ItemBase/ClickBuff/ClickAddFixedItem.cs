using UnityEngine;

[CreateAssetMenu(fileName = "ClickAddFixedItem", menuName = "Clicker/Item/Click/Add/Fixed Value")]
public class ClickAddFixedItem : ItemData
{
    [Tooltip("クリック加算に足す固定値")]
    public double addend = 10.0;

    public override void ApplyEffect(ItemManager manager)
    {
        manager.moneyClickManager.AddClickAddend(addend);
    }
}
