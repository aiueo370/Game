//using UnityEngine;
//using UnityEngine.UI;
//using TMPro; // TextMeshPro を使わないなら外してOK

//public class BuyItemButton : MonoBehaviour
//{
//    [Header("Optional UI")]
//    [SerializeField] private Text priceText;       // 価格表示
//    [SerializeField] private Button buyButton;         // ボタン有効/無効
//    [SerializeField] private GameObject infoPanelRoot; // マウスオーバーパネル

//    private ItemManager itemManager;
//    private BuyItemButtonsController owner;
//    [SerializeField] private ItemData currentItem;

//    public ItemData CurrentItem => currentItem; // ツールチップ用に参照を公開

//    /// <summary>親からアイテムを割り当て</summary>
//    public void Bind(ItemManager manager, BuyItemButtonsController parent, ItemData item)
//    {
//        itemManager = manager;
//        owner = parent;
//        currentItem = item;
//        UpdateView();
//    }

//    /// <summary>未割当状態に戻す</summary>
//    public void Unbind()
//    {
//        currentItem = null;
//        UpdateView();
//    }

//    public void OnClickedBuy()
//    {
//        if (currentItem == null || itemManager == null) return;

//        // ItemManagerが構造体リストで管理している想定（TryPurchaseは前に渡した実装）
//        bool ok = itemManager.TryPurchase(currentItem);
//        if (ok)
//        {
//            // 購入できたら親に通知して再配置
//            owner?.OnAnyButtonPurchased();
//        }
//        else
//        {
//            // 失敗時のSE/演出などあればここで
//        }
//    }

//    private void Update()
//    {
//        // インジケータ更新（所持金による押下可否など）
//        if (buyButton != null && itemManager != null && currentItem != null)
//        {
//            // ItemManagerに直接参照がない場合は、ItemManager側に affordability を返すAPIを用意してもOK
//            var canAfford = itemManager.moneyClickManager != null // 参照大丈夫かの簡易チェック
//                            && itemManager.TryGetComponent<MoneyRepository>(out var _); // ここはプロジェクトに合わせて
//            // ↑MoneyRepositoryはItemManagerの中にSerializeFieldで持っているなら
//            //   ItemManager側に public bool CanAfford(ItemData item) を用意して呼ぶ形が綺麗です。
//            buyButton.interactable = true; // ひとまず常に有効（後でCanAffordに差し替え）
//        }
//    }

//    private void UpdateView()
//    {
//        if (priceText != null)
//        {
//            priceText.text = (currentItem != null) ? FormatMoney(currentItem.price) : "-";
//        }

//        // 任意：パネルの初期状態
//        if (infoPanelRoot != null)
//            infoPanelRoot.SetActive(false);
//    }

//    private static string FormatMoney(double v)
//    {
//        return MoneyFormatter.ToSuffixString(v,1) + " $";
//    }

//    // マウスオーバーでツールチップ表示したい場合は後でここに実装する：
//    // IPointerEnterHandler / IPointerExitHandler を実装して infoPanelRoot を開閉、等
//}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyItemButton : MonoBehaviour
{
    [Header("Optional UI")]
    [SerializeField] private Text priceText;
    [SerializeField] private Button buyButton;

    [Header("Tint when unaffordable")]
    [SerializeField] private Image selfImage;                  // 同オブジェクトの Image。未設定なら自動取得
    [SerializeField] private Color unaffordableColor = Color.red;

    [SerializeField] AudioClip submitSE;
    [SerializeField] AudioClip missedSE;

    private AudioSource audioSource;

    private Color affordableColor;                             // 元の色を覚えておく

    private Color priceTextDefaultColor;


    private ItemManager itemManager;
    private BuyItemButtonsController owner;
    private ItemData currentItem;

    // 所持金参照（ItemManagerにCanAffordが無い場合のフォールバック用）
    [SerializeField] private MoneyRepository repository;

    public ItemData CurrentItem => currentItem;

    public void Bind(ItemManager manager, BuyItemButtonsController parent, ItemData item)
    {
        itemManager = manager;
        owner = parent;
        currentItem = item;
        UpdateView();
        UpdateAffordanceTint();
    }

    public void Unbind()
    {
        currentItem = null;
        UpdateView();
        UpdateAffordanceTint();
    }

    private void Awake()
    {
        if (selfImage == null) selfImage = GetComponent<Image>();
        if (selfImage != null) affordableColor = selfImage.color;

        if (repository == null) repository = FindFirstObjectByType<MoneyRepository>();

        if (priceText != null) priceTextDefaultColor = priceText.color;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 毎フレーム：買える/買えないの見た目を更新
        UpdateAffordanceTint();
    }

    public void OnClickedBuy()
    {
        if (currentItem == null || itemManager == null) return;

        bool ok = itemManager.TryPurchase(currentItem);
        if (ok)
        {
            owner?.OnAnyButtonPurchased();      // 親に再配置を依頼
            audioSource.PlayOneShot(submitSE);
        }
        else
        {
            audioSource.PlayOneShot(missedSE);
        }
    }

    private void UpdateView()
    {
        if (priceText != null)
        {
            priceText.text = (currentItem != null)
                ? FormatMoney(GetPriceSafe(currentItem))
                : "-";
        }
    }

    private void UpdateAffordanceTint()
    {
        bool canAfford = CanAffordCurrent();

        // ボタンのインタラクティブ切替（任意）
        if (buyButton != null) buyButton.interactable = canAfford;

        // 画像の色を切替
        if (selfImage != null)
            selfImage.color = canAfford ? affordableColor : unaffordableColor;

        // 文字色も切り替え
        if (priceText != null)
            priceText.color = canAfford ? priceTextDefaultColor : Color.white;

    }

    // —— 価格取得（ItemDataにGetPriceが無ければ price を使う）——
    private double GetPriceSafe(ItemData item)
    {
        // ItemData に GetPrice(ItemManager) を生やしているなら：
        // return item.GetPrice(itemManager);
        return item.price;
    }

    // —— 購入可否判定 —— 
    private bool CanAffordCurrent()
    {
        if (currentItem == null) return false;

        // 1) ItemManager に CanAfford があるならそちらを使うのが綺麗
        // return itemManager != null && itemManager.CanAfford(currentItem);

        // 2) フォールバック：MoneyRepository を直接参照
        if (repository == null) return false;
        double price = GetPriceSafe(currentItem);
        return repository.totalAssets >= price;
    }

    private static string FormatMoney(double v)
    {
        string[] u = { "", "K", "M", "B", "T" };
        int i = 0;
        while (i < u.Length - 1 && System.Math.Abs(v) >= 1000.0) { v /= 1000.0; i++; }
        return v.ToString("0.###") + u[i] + " $";
    }
}
