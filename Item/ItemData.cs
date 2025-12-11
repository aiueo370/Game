//using UnityEngine;

//// アイテムの種類を定義
//public enum ItemType
//{
//    GlobalMultiplier,   // 全施設の収入倍率アップ
//    FacilityMultiplier, // 特定の施設の収入倍率アップ
//    ClickPower          // クリック収入を強化
//}

//[CreateAssetMenu(fileName = "NewItem", menuName = "Clicker/ItemData")]
//public class ItemData : ScriptableObject
//{
//    public string itemName;      // 名前
//    public string description;   // 説明文（UI表示用）
//    public double price;         // 購入価格
//    public bool isPurchased = false;  // 購入済みかどうかのフラグ（初期は false）

//    public ItemType type;        // アイテムの種類（GlobalMultiplier / FacilityMultiplier / ClickPower）
//    public double multiplier = 1.0;     // アイテム効果の倍率

//    public FacilityName targetFacility;  // FacilityMultiplier 用の対象施設
//}
using UnityEngine;
using System;

public abstract class ItemData : ScriptableObject
{
    [Header("Common")]
    public string itemId;            // 進行状況を紐づける安定ID
    public string itemName;
    [TextArea] public string description;
    public double price;

#if UNITY_EDITOR
    // インスペクタで空なら自動採番（Editorのみ）
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemId))
        {
            itemId = Guid.NewGuid().ToString("N");
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    /// <summary>購入後に適用する効果。</summary>
    public abstract void ApplyEffect(ItemManager manager);
}
