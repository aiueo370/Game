using UnityEngine;

public class MoneyClickManager : MonoBehaviour
{
    [SerializeField] private double baseClickValue = 10;
    [SerializeField] private MoneyRepository moneyRepository;

    private double extraClickAddend = 0.0; // â¡éZ
    private double clickMultiplier = 1.0;  // èÊéZ

    public void OnClicked()
    {
        double total = (baseClickValue + extraClickAddend) * clickMultiplier;
        moneyRepository.AddMoney(total);
    }

    public void AddClickAddend(double add)
    {
        extraClickAddend += add;
    }

    public void MultiplyClick(double factor)
    {
        clickMultiplier *= factor;
    }

    public double GetPreviewValue()
    {
        return (baseClickValue + extraClickAddend) * clickMultiplier;
    }
}
