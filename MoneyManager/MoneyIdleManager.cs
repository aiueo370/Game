//using System.Collections.Generic;
//using UnityEngine;

//public class MoneyIdleManager : MonoBehaviour
//{
//    [SerializeField] double moneyPerSecond = 1;
//    [SerializeField] MoneyRepository repository;

//    [SerializeField] DollarPerSecondDisplay dispValue;  // 秒間の増加量

//    [SerializeField] FacilityManager facilityManager;   // 施設群

//    private void Awake()
//    {
//        ChangeMoneyPerSecond();
//    }

//    private void Update()
//    {
//        repository.AddMoney(moneyPerSecond * Time.deltaTime);
//    }

//    // 秒間の生産量の変更
//    public void ChangeMoneyPerSecond()
//    {
//        //　FacilityName name;
//        moneyPerSecond = 0;

//        for (int i = 0; i < facilityManager.facilities.Count; i++)
//        {
//            double production = facilityManager.facilities[i].Production;
//            // double buff = ~~~;

//            // production *= buff;

//            // moneyPerSecond = production * buff;
//            moneyPerSecond += production;
//        }

//        // 秒間生産量の表示量を変更
//        dispValue.SetDollarPerSecond(moneyPerSecond);
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class MoneyIdleManager : MonoBehaviour
{
    [SerializeField] private double moneyPerSecond = 1;
    [SerializeField] private MoneyRepository repository;

    [SerializeField] private DollarPerSecondDisplay dispValue;   // 秒間の増加量表示
    [SerializeField] private FacilityManager facilityManager;    // 施設群

    // ★ 追加：アイテム／バフ参照用
    [SerializeField] private ItemManager itemManager;

    private void Awake()
    {
        ChangeMoneyPerSecond();
    }

    private void Update()
    {
        repository.AddMoney(moneyPerSecond * Time.deltaTime);
    }

    /// <summary>
    /// 秒間の生産量の再計算
    /// moneyPerSecond = Σ( facility.Production * facilityFactor(name) * globalFactor )
    /// </summary>
    [ContextMenu("Recalculate Per Second")]
    public void ChangeMoneyPerSecond()
    {
        // 安全チェック
        if (facilityManager == null || facilityManager.facilities == null)
        {
            moneyPerSecond = 0;
            if (dispValue != null)
            {
                dispValue.SetDollarPerSecond(moneyPerSecond);
            }
            return;
        }

        // 所持数参照デリゲート（ItemManager が無ければ 0 を返す）
        int GetCount(FacilityName n)
        {
            if (itemManager == null) return 0;
            return itemManager.GetFacilityCount(n);
        }

        // 全体係数（ItemManager or Registry が無ければ 1.0）
        double globalFactor = 1.0;
        if (itemManager != null && itemManager.productionBuffRegistry != null)
        {
            globalFactor = itemManager.productionBuffRegistry.ComputeGlobalFactor(GetCount);
        }

        // 合計
        double total = 0.0;

        for (int i = 0; i < facilityManager.facilities.Count; i++)
        {
            var f = facilityManager.facilities[i];

            // 素の生産量（1秒あたり、所持数込み想定）
            double baseProduction = f.production;

            // 施設係数（ItemManager or Registry が無ければ 1.0）
            double facilityFactor = 1.0;
            if (itemManager != null && itemManager.productionBuffRegistry != null && f.data != null)
            {
                FacilityName name = f.data.FacilityName; // FacilityData 側の大文字プロパティ想定
                facilityFactor = itemManager.productionBuffRegistry.ComputeFacilityFactor(name, GetCount);
            }

            total += baseProduction * facilityFactor * globalFactor;
        }

        moneyPerSecond = total;

        // 表示更新
        if (dispValue != null)
        {
            dispValue.SetDollarPerSecond(moneyPerSecond);
        }
    }
}
