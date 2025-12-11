using UnityEngine;

public enum FacilityName
{
    OnlineShop,             // 個人ネットショップ
    ConvenienceStore,       // コンビニ
    ShoppingMall,           // ショッピングモール
    Bank,                   // 銀行
    GlobalCorporation,      // 多国籍企業
    Nation,                 // 国家
    PlanetEarth,            // 地球
    ParallelWorld,          // 並行世界
    GalacticFederation,     // 銀河連合
    TimeBank,               // 時空銀行
}

[CreateAssetMenu(fileName = "FacilityData", menuName = "Clicker/FacilityData")]
public class FacilityData : ScriptableObject
{
    public FacilityName FacilityName;       // アイテム名（例: 工場）
    public double baseProduction; // 1個あたりの基礎生産量
    public double basePrice;      // 初期価格
    public double priceIncrease;  // 価格上昇率（例: 1.15 → 15%上がる）
}