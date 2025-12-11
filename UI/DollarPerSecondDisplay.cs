using TMPro;
using UnityEngine;

public class DollarPerSecondDisplay : MonoBehaviour
{
    [SerializeField] private double dollarPerSecValue = 0;

    TextMeshProUGUI m_TextMeshPro;

    private void Awake()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        string display = MoneyFormatter.ToSuffixString(dollarPerSecValue, 1);
        m_TextMeshPro.text = display;
    }

    public void SetDollarPerSecond(double _dollarPerSecValue)
    {
        dollarPerSecValue = _dollarPerSecValue;
    }
}
