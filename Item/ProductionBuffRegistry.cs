using System;
using System.Collections.Generic;

public class ProductionBuffRegistry
{
    // {İ•Ê‚ÌŒÅ’è”{—¦i‰Šú=1j
    private readonly Dictionary<FacilityName, double> facilityFixedMultiplier = new();

    // {İ•Ê ~ ‘¼{İŠ”ˆË‘¶itarget © source ‚Ìu1‘Ì‚ ‚½‚è‰ÁZŒW”vj
    // ƒoƒtŒW” = 1 + ƒ°( perUnitAddend[target][source] * count(source) )
    private readonly Dictionary<FacilityName, Dictionary<FacilityName, double>> perTargetPerSourceAddend = new();

    // ‘S‘Ì‚ÌŒÅ’è”{—¦i‰Šú=1j
    private double globalFixedMultiplier = 1.0;

    // ‘S‘Ì ~ ‘¼{İŠ”ˆË‘¶isource‚²‚Æ‚Ì1‘Ì‚ ‚½‚è‰ÁZŒW”j
    private readonly Dictionary<FacilityName, double> globalPerSourceAddend = new();

    public void AddFacilityFixedMultiplier(FacilityName target, double factor)
    {
        if (!facilityFixedMultiplier.ContainsKey(target))
        {
            facilityFixedMultiplier[target] = 1.0;
        }
        facilityFixedMultiplier[target] *= factor;
    }

    public void AddFacilityPerUnitAddend(FacilityName target, FacilityName source, double perUnitAddend)
    {
        if (!perTargetPerSourceAddend.ContainsKey(target))
        {
            perTargetPerSourceAddend[target] = new Dictionary<FacilityName, double>();
        }
        if (!perTargetPerSourceAddend[target].ContainsKey(source))
        {
            perTargetPerSourceAddend[target][source] = 0.0;
        }
        perTargetPerSourceAddend[target][source] += perUnitAddend;
    }

    public void AddGlobalFixedMultiplier(double factor)
    {
        globalFixedMultiplier *= factor;
    }

    public void AddGlobalPerUnitAddend(FacilityName source, double perUnitAddend)
    {
        if (!globalPerSourceAddend.ContainsKey(source))
        {
            globalPerSourceAddend[source] = 0.0;
        }
        globalPerSourceAddend[source] += perUnitAddend;
    }

    public double ComputeGlobalFactor(Func<FacilityName, int> getCount)
    {
        double countFactor = 1.0;
        foreach (var kv in globalPerSourceAddend)
        {
            int c = getCount(kv.Key);
            countFactor += kv.Value * c; // 1 + ƒ°(k * count)
        }
        return globalFixedMultiplier * countFactor;
    }

    public double ComputeFacilityFactor(FacilityName target, Func<FacilityName, int> getCount)
    {
        double fixedMul = facilityFixedMultiplier.TryGetValue(target, out var f) ? f : 1.0;

        double countFactor = 1.0;
        if (perTargetPerSourceAddend.TryGetValue(target, out var map))
        {
            foreach (var kv in map)
            {
                int c = getCount(kv.Key);
                countFactor += kv.Value * c; // 1 + ƒ°(k * count)
            }
        }
        return fixedMul * countFactor;
    }

    public void ResetAll()
    {
        facilityFixedMultiplier.Clear();
        perTargetPerSourceAddend.Clear();
        globalFixedMultiplier = 1.0;
        globalPerSourceAddend.Clear();
    }
}
