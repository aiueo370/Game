using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FacilityManager facilityManager;   // 施設の実データ元
    [SerializeField] private MoneyRepository moneyRepository;
    [SerializeField] public MoneyClickManager moneyClickManager;

    [Header("Production Buffs")]
    public ProductionBuffRegistry productionBuffRegistry = new ProductionBuffRegistry();

    // ───────────── Item + Purchased を1要素で管理 ─────────────
    [System.Serializable]
    public struct ItemEntry
    {
        public ItemData item;     // ScriptableObject（効果は item.ApplyEffect(this) で適用）
        public bool purchased;    // 購入済みか（Inspectorで操作可）
    }

    [Header("Items (SO + Purchased)")]
    [Tooltip("ここにアイテムSOと購入フラグを入れる。起動時に 'purchased' が true のものは自動適用されます。")]
    public List<ItemEntry> items = new();

    [Header("Apply Options")]
    [Tooltip("起動時に items の purchased==true を適用する")]
    [SerializeField] private bool applyOnStart = true;

    // 施設一覧（FacilityManager から重複なく抽出）
    private readonly List<FacilityData> allFacilitiesCache = new();
    public IReadOnlyList<FacilityData> AllFacilities => allFacilitiesCache;

    // すでに効果を適用済みのSOを記録（重複適用の防止）
    private readonly HashSet<ItemData> appliedSet = new();

    // ───────────── Unity lifecycle ─────────────
    private void Awake()
    {
        RebuildAllFacilitiesCache();
    }

    private void Start()
    {
        if (applyOnStart)
            ApplyPurchasedFromInspectorList();   // Inspectorの purchased==true を反映
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            RebuildAllFacilitiesCache();
    }
#endif

    // ───────────── Facility 取得系 ─────────────
    /// <summary>FacilityManager から FacilityData 一覧を再構築（FacilityNameで重複排除）</summary>
    public void RebuildAllFacilitiesCache()
    {
        allFacilitiesCache.Clear();
        if (facilityManager == null || facilityManager.facilities == null) return;

        for (int i = 0; i < facilityManager.facilities.Count; i++)
        {
            var entry = facilityManager.facilities[i];
            var data = (entry != null) ? entry.data : null;
            if (data == null) continue;

            bool exists = false;
            for (int j = 0; j < allFacilitiesCache.Count; j++)
            {
                var d = allFacilitiesCache[j];
                if (d != null && d.FacilityName == data.FacilityName)
                {
                    exists = true; break;
                }
            }
            if (!exists) allFacilitiesCache.Add(data);
        }
    }

    /// <summary>指定施設の所持数（FacilityManager 由来）</summary>
    public int GetFacilityCount(FacilityName name)
    {
        if (facilityManager == null || facilityManager.facilities == null) return 0;

        for (int i = 0; i < facilityManager.facilities.Count; i++)
        {
            var e = facilityManager.facilities[i];
            if (e == null || e.data == null) continue;
            if (e.data.FacilityName == name)
                return e.count;   // ← プロジェクトの実フィールド名に合わせて
        }
        return 0;
    }

    // ───────────── Inspector反映＆購入API ─────────────
    /// <summary>
    /// 起動時/手動で、items の purchased==true を効果適用（重複適用は防止）
    /// </summary>
    [ContextMenu("Apply Purchased From Inspector List")]
    public void ApplyPurchasedFromInspectorList()
    {
        if (items == null) return;

        foreach (var entry in items)
        {
            if (entry.item == null || !entry.purchased) continue;
            if (appliedSet.Add(entry.item))     // まだ適用していなければ
                entry.item.ApplyEffect(this);
        }
    }

    /// <summary>
    /// 指定SOが購入済みかどうか（Inspectorの items を参照）
    /// </summary>
    public bool IsPurchased(ItemData target)
    {
        if (target == null || items == null) return false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == target)
                return items[i].purchased;
        }
        return false;
    }

    /// <summary>
    /// 指定SOを購入処理（所持金チェック→効果適用→リストの purchased を true に更新）
    /// </summary>
    public bool TryPurchase(ItemData target)
    {
        if (target == null || items == null || moneyRepository == null) return false;

        for (int i = 0; i < items.Count; i++)
        {
            var e = items[i];
            if (e.item != target) continue;

            if (e.purchased) return false;                          // 既に購入済み
            if (moneyRepository.totalAssets < target.price) return false;
            if (!moneyRepository.UseMoney(target.price)) return false;

            target.ApplyEffect(this);                               // 効果適用
            appliedSet.Add(target);

            e.purchased = true;                                     // リストの要素（struct）を上書き
            items[i] = e;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            return true;
        }
        return false; // リストに未登録
    }

    /// <summary>
    /// 既存の購入済みを解除（デバッグ用）※効果の巻き戻しはしない。必要なら Registry をリセットして再適用を。
    /// </summary>
    public bool UndoPurchase(ItemData target)
    {
        if (target == null || items == null) return false;

        for (int i = 0; i < items.Count; i++)
        {
            var e = items[i];
            if (e.item != target) continue;

            if (!e.purchased) return false;

            e.purchased = false;
            items[i] = e;
            appliedSet.Remove(target);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            return true;
        }
        return false;
    }

    /// <summary>
    /// 全効果をリセットして、現在の purchased==true から再適用（デバッグ用）
    /// </summary>
    [ContextMenu("Reapply All Purchased (Reset Buffs)")]
    public void ReapplyAllPurchased()
    {
        productionBuffRegistry.ResetAll();
        appliedSet.Clear();

        // クリック系の値をリセットしたい場合はここで初期化APIを用意して呼ぶ
        // 例）moneyClickManager.ResetBonuses();

        ApplyPurchasedFromInspectorList();
    }

    public bool CanAfford(ItemData target)
    {
        if (target == null || moneyRepository == null) return false;
        return moneyRepository.totalAssets >= target.price; // 動的価格ならここで計算
    }

}
