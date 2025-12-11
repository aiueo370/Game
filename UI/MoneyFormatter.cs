using UnityEngine;

public static class MoneyFormatter
{
    // サフィックスリスト
    private static readonly string[] suffixes = { "", "K", "M", "G", "T", "P", "E" };

    public static string ToSuffixString(double value, int decimals = 2)
    {
        int index = 0;

        // 1000以上なら割ってサフィックスを進める
        while (value >= 1000 && index < suffixes.Length - 1)
        {
            value /= 1000d;
            index++;
        }

        return value.ToString("F" + decimals) + suffixes[index];
    }
}
