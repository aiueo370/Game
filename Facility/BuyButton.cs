using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] FacilityName facilityName;
    [SerializeField] BuyButtonUIController uIController;

    [SerializeField] private Text facilityNameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text countText;
    [SerializeField] private Text baseDpsText;

    // BuySE
    [SerializeField] private AudioClip boughtSe;
    [SerializeField] private AudioClip missSe;

    public bool isVisible;
    CanvasGroup cg;

    private FacilityManager manager;
    private MoneyRepository repository;

    private double basePrice;
    private double unlockMultiplier = 0.5;
    private double unlockPrice;

    // 音を鳴らやつ
    private AudioSource audioSource;

    private void Awake()
    {
        manager = FindFirstObjectByType<FacilityManager>();     // マネージャーの取得
        repository = FindFirstObjectByType<MoneyRepository>();  // リポジトリの取得

        // ベース価格のunlockMultiplier倍の値段なら購入可能
        basePrice = manager.GetBasePrice((int)facilityName);    // ベース価格の取得
        unlockPrice = basePrice * unlockMultiplier;

        isVisible = CanVisible();

        cg = GetComponent<CanvasGroup>();
        VisibleSwitcher();

        // UIの初期化
        SetFacilityNameText();
        int index = (int)facilityName;
        SetPriceText(index);
        SetCountText(index);
        SetBaseDollarsPerSecond(index);

        // SE用のAudioSourceの初期化
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // isVisbleの更新
        if (isVisible == false)
        {
            if (CanVisible())
            {
                isVisible = true;
                uIController.AddVisibleButton();
            }
        }

        VisibleSwitcher();
    }

    public void OnClickBuyFacility()
    {
        int index = (int)facilityName;
        // 変えたならUIも変える
        if(manager.BuyFacility(index))
        {
            SetPriceText(index);
            SetCountText(index);

            // SEの再生
            audioSource.PlayOneShot(boughtSe);
        }
        else
        {
            // SEの再生
            audioSource.PlayOneShot(missSe);
        }
    }

    private bool CanVisible()
    {
        return
            repository.maxHeldMoney > unlockPrice ||          // 所持最高額が解放額より高い
            facilityName == FacilityName.OnlineShop ||        // オンラインショップは常に可視
            manager.facilities[(int)facilityName].count > 0;  // 1つ以上所持している
    }

    private void VisibleSwitcher()
    {
        if (isVisible == false)
        {
            cg.alpha = 0f;           // 見た目を消す
            cg.interactable = false; // 押せない
            cg.blocksRaycasts = false; // 当たり判定も無効
        }
        else
        {
            cg.alpha = 1f;           // 見た目を消す
            cg.interactable = true; // 押せない
            cg.blocksRaycasts = true; // 当たり判定も無効
        }
    }


    private void SetFacilityNameText()
    {
        facilityNameText.text = FacilityNameExtensions.ToLabel(facilityName);
    }

    private void SetPriceText(int _index)
    {
        double price = manager.GetCurrentPrice(_index);
        priceText.text = MoneyFormatter.ToSuffixString(price);
    }

    private void SetCountText(int _index)
    {
        int count = manager.facilities[_index].count;
        countText.text = count.ToString();
    }

    private void SetBaseDollarsPerSecond(int _index)
    {
        double dps = manager.facilities[_index].data.baseProduction;
        baseDpsText.text = MoneyFormatter.ToSuffixString(dps);
    }
}
