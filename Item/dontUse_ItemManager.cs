//using UnityEngine;
//using System.Collections.Generic;

//public class ItemManager : MonoBehaviour
//{
//    public List<ItemData> allItems;           // ゲーム内で使える全アイテム
//    public List<FacilityData> allFacilities;  // 全施設のデータ
//    public MoneyClickManager moneyClickManager; // クリック収入管理
//    [SerializeField] private MoneyRepository moneyRepository; // お金の管理

//    // アイテムを購入する処理
//    public void BuyItem(ItemData item)
//    {
//        if (item.isPurchased) return;                 // 既に購入済みなら処理しない
//        if (moneyRepository.totalAssets < item.price) return; // 所持金不足なら処理しない
//        if (!moneyRepository.UseMoney(item.price)) return;    // お金を使えなければ処理しない

//        ApplyItemEffect(item);  // アイテムの効果を適用
//        item.isPurchased = true; // 購入済みにする
//    }

//    // アイテム効果を適用する処理
//    private void ApplyItemEffect(ItemData item)
//    {
//        switch (item.type)
//        {
//            case ItemType.GlobalMultiplier:
//                foreach (var facility in allFacilities)
//                    facility.baseProduction *= item.multiplier; // 全施設の生産量に倍率をかける
//                break;

//            case ItemType.FacilityMultiplier:
//                var target = allFacilities.Find(f => f.FacilityName == item.targetFacility);
//                if (target != null)
//                    target.baseProduction *= item.multiplier; // 指定施設だけ倍率をかける
//                break;

//            case ItemType.ClickPower:
//                if (moneyClickManager != null)
//                    moneyClickManager.clickMultiplier *= item.multiplier; // クリック収入を増やす
//                break;
//        }
//    }
//}
