using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemButtonsController : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private List<BuyItemButton> buttons = new List<BuyItemButton>(); // Inspectorに3つ入れる

    private void Reset()
    {
        // 子から自動取得（足りない場合はInspectorで手動調整OK）
        buttons = GetComponentsInChildren<BuyItemButton>(true).ToList();
    }

    private void OnEnable()
    {
        Refresh();
    }

    /// <summary>未購入アイテムの中から最安値3つをボタンに割り当て</summary>
    public void Refresh()
    {
        if (itemManager == null || itemManager.items == null) return;

        // 未購入 & SOあり を抽出して価格で昇順
        var cheapest = itemManager.items
            .Where(e => e.item != null && !e.purchased)
            .OrderBy(e => GetPriceSafe(e.item))
            .Take(buttons.Count)
            .Select(e => e.item)
            .ToList();

        // 割り当て（足りない分は非表示）
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < cheapest.Count)
            {
                buttons[i].Bind(itemManager, this, cheapest[i]);
                buttons[i].gameObject.SetActive(true);
            }
            else
            {
                buttons[i].Unbind();
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    // 購入後に再配置したいのでボタンから呼んでもらう
    public void OnAnyButtonPurchased()
    {
        Refresh();
    }

    // ItemDataにGetPriceを実装していない場合のフォールバック
    private static double GetPriceSafe(ItemData item)
    {
        // もし ItemData に GetPrice(ItemManager) を実装しているならそれを使う様にしてもOK
        return item.price;
    }
}
