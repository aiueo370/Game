using UnityEngine;
using UnityEngine.InputSystem;

public class BuyButtonUIController : MonoBehaviour
{
    [SerializeField] private BuyButton[] buyButtons;

    [SerializeField] private double scrollSpeed;

    // ボタンのTransform周りな変数
    private double buttonHeight = 172.5;
    private double buyButtonAreaHeight = 690;

    // スクロールに関する変数
    private double baseHeight;
    private double addHeight;
    private double scrollHeightMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalclateScrollHeightMax();

        // ボタンの初期位置を決定
        for (int i = 0; i < buyButtons.Length; i++)
        {
            RectTransform rt = buyButtons[i].GetComponent<RectTransform>();
            float y = (float)(buttonHeight / 2 - buttonHeight * i) - (float)(buttonHeight / 2);
            rt.anchoredPosition = new Vector2(0f, y);
        }

        // 自身の初期Y座標を基準の高さとして記録
        baseHeight = transform.position.y;
        addHeight = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        ScrollButtons();
    }

    // 見えているボタンの合計高さを取得
    double CalculateButtonHeight()
    {
        double result = 0;

        int isVisibleButtonNum = SumVisibleButtonNum();
        result = buttonHeight * (float)isVisibleButtonNum;

        return result;
    }

    // 現状のボタンの合計数を計算
    private int SumVisibleButtonNum()
    {
        int result = 0;

        foreach (var buyButton in buyButtons)
        {
            if(buyButton.isVisible) result++;
        }

        return result;
    }

    // 可視ボタンが増えたことを通知する
    public void AddVisibleButton()
    {
        CalclateScrollHeightMax();
    }

    // ボタンをマウスカーソルでスクロールする
    private void ScrollButtons()
    {
        // 新Input Systemのスクロール値を取得（1ノッチ ≒ 120 を想定して正規化）
        float scrollNotch = 0f;
        if (Mouse.current != null)
        {
            // scroll はピクセル単位っぽい値（多くの環境で1ノッチ=±120）
            float raw = Mouse.current.scroll.ReadValue().y;
            scrollNotch = -(raw / 120f);
        }

        if (Mathf.Abs(scrollNotch) > 0.001f)
        {
            // 加算値を更新（double のまま計算）
            addHeight += scrollNotch * (float)scrollSpeed;

            // 上下限をクランプ（double対応。Mathf.Clamp は float 用）
            addHeight = System.Math.Max(0.0, System.Math.Min(addHeight, scrollHeightMax));

            // 位置反映（基準 + 加算）
            var pos = transform.position;
            pos.y = (float)(baseHeight + addHeight);
            transform.position = pos;
        }
    }

    // スクロール可能な高さを取得
    private void CalclateScrollHeightMax()
    {
        double allButtonsHeight = CalculateButtonHeight();

        scrollHeightMax = allButtonsHeight - buyButtonAreaHeight;
        scrollHeightMax -= buttonHeight / 2;

        if (scrollHeightMax < 0) scrollHeightMax = 0;

        //Debug.Log(scrollHeightMax);
    }
}