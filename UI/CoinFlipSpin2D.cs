using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CoinFlipSpin2D : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite frontSprite;      // 表面
    [SerializeField] private Sprite backSprite;       // 裏面

    [Header("Flip Motion")]
    [SerializeField] private float degreesPerSecond = 720f; // コインの“回転速度”（見かけ上）
    [SerializeField, Range(0f, 1f)] private float minEdgeScale = 0.08f; // 真横の時の最小厚み
    [SerializeField] private bool useUnscaledTime = false;  // ポーズ中も回したいなら true

    private SpriteRenderer sr;
    private Vector3 baseScale;
    private float angleDeg;       // 見かけの回転角（Y回転相当）
    private bool showingBack;     // 現在 裏面 表示中か？

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        angleDeg = 0f;
        showingBack = false;
        if (frontSprite != null) sr.sprite = frontSprite;
        // 念のため元スケールに戻す
        transform.localScale = baseScale;
    }

    private void Update()
    {
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        angleDeg += degreesPerSecond * dt;

        // コサインで “厚み” を表現：±1 → 1、 0付近（横向き）→ 0 に近づく
        float cos = Mathf.Cos(angleDeg * Mathf.Deg2Rad);

        // 横向きで完全に消えないよう下限を入れる
        float xScale = Mathf.Max(Mathf.Abs(cos), minEdgeScale);

        // Xだけ薄く/厚くしてコインの厚み表現
        Vector3 s = baseScale;
        s.x = baseScale.x * xScale;
        transform.localScale = s;

        // 表裏の切替：cos < 0 の半周期は裏面、それ以外は表面
        bool nowBack = (cos < 0f);
        if (nowBack != showingBack)
        {
            sr.sprite = nowBack ? backSprite : frontSprite;
            showingBack = nowBack;
        }
    }
}
