[System.Serializable]
public class Facility
{
    public FacilityData data; // ’è‹`ƒf[ƒ^
    public int count = 0; // Š”

    // Œ»İ‰¿Ši
    public double currentPrice
    {
        get
        {
            return data.basePrice * System.Math.Pow(data.priceIncrease, count);
        }
    }

    // 1•b‚ ‚½‚è‚Ì¶Y—Ê
    public double production
    {
        get
        {
            return data.baseProduction * count;
        }
    }
}