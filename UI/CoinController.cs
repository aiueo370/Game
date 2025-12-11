using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private AudioClip[] clickSounds;   // 鳴らす音配列　ランダムで決定する
    private AudioSource audioSource;

    // コインを飛ばすのに使うあれこれ
    [SerializeField] private GameObject coinPrefab;       // Rigidbody2D付きのコインPrefab
    [SerializeField] private Transform spawnOrigin;       // 未指定なら自分の位置
    [SerializeField] private int spawnCount = 5;          // 一度に飛ばす数
    [SerializeField] private float coinLifetime = 2f;     // 自動破棄時間(秒・0以下で無効)

    [SerializeField] private Vector2 forceRange = new Vector2(3f, 6f); // インパルスの強さ
    [SerializeField] private float angleSpread = 360f;    // 角度拡散(度)。360なら全方位
    [SerializeField] private float torqueRange = 180f;    // 角速度のランダム幅(±)


    private int lastIndex = -1; // 前回選んだインデックス

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClicked()
    {
        PlayClickSound();
        SpawnAndShootCoins();
    }


    private void SpawnAndShootCoins()
    {
        if (coinPrefab == null) return;

        // 生成位置を決定
        Vector3 spawnPos;
        
        spawnPos = (spawnOrigin != null) ? spawnOrigin.position : transform.position;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            if (coinLifetime > 0f)
            {
                Destroy(coin, coinLifetime);
            }

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            // ランダム方向（上を基準に±半分のスプレッド）
            float angle = Random.Range(-angleSpread * 0.5f, angleSpread * 0.5f);
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * Vector2.up; // “上”中心の散り

            float force = Random.Range(forceRange.x, forceRange.y);
            rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

            // ランダム回転も足してリッチに
            float torque = Random.Range(-torqueRange, torqueRange);
            rb.AddTorque(torque, ForceMode2D.Impulse);
        }
    }

    // AudioClip配列の中からランダムで再生　前回と同じ音は再生されない
    private void PlayClickSound()
    {
        if (clickSounds == null || clickSounds.Length == 0) return;

        int index;

        if (clickSounds.Length == 1)
        {
            index = 0;
        }
        else
        {
            // 前回と違うインデックスを選ぶ
            do
            {
                index = Random.Range(0, clickSounds.Length);
            } while (index == lastIndex);
        }

        lastIndex = index;
        audioSource.PlayOneShot(clickSounds[index]);
    }


}
