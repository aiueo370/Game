public static class FacilityNameExtensions
{
    public static string ToLabel(this FacilityName name)
    {
        return name switch
        {
            FacilityName.OnlineShop => "オンラインショップ",
            FacilityName.ConvenienceStore => "コンビニ",
            FacilityName.ShoppingMall => "ショッピングモール",
            FacilityName.Bank => "銀行",
            FacilityName.GlobalCorporation => "多国籍企業",
            FacilityName.Nation => "国家",
            FacilityName.PlanetEarth => "地球",
            FacilityName.ParallelWorld => "並行世界",
            FacilityName.GalacticFederation => "銀河連合",
            FacilityName.TimeBank => "時空銀行",
            _ => name.ToString()
        };
    }
}
