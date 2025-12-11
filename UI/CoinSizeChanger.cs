using UnityEngine;

public class CoinSizeChangerUI : MonoBehaviour
{
    [SerializeField] private float baseScaleRate = 1f;  // ÉxÅ[ÉXî{ó¶
    [SerializeField] private float amplitude = 0.2f;    // êUÇÍïù
    [SerializeField] private float sinWaveSpeed = 1f;   // óhÇÍÇÈë¨Ç≥

    private RectTransform rectTransform;
    private Vector3 baseScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        baseScale = rectTransform.localScale;
    }

    private void Update()
    {
        float wave = Mathf.Sin(Time.time * 2f * sinWaveSpeed) * amplitude;
        float scaleRate = baseScaleRate + wave;

        rectTransform.localScale = baseScale * scaleRate;
    }
}
