using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MoneyValueDisplay : MonoBehaviour
{
    [SerializeField] private double moneyValue = 0;
    [SerializeField] private MoneyRepository moneyRepository;
    
    TextMeshProUGUI m_TextMeshPro;

    private void Awake()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        moneyValue = moneyRepository.GetTotalAssets();

        string display = MoneyFormatter.ToSuffixString(moneyValue, 1);
        m_TextMeshPro.text = display;
    }

    public void SetMoneyValue(double _moneyValue)
    {
        moneyValue = _moneyValue;
    }
}
