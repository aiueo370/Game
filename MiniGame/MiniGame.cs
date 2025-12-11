using UnityEngine;

// 施設ごとのミニゲームの基底クラス
public abstract class MiniGame : MonoBehaviour
{
    public string FacilityName; // このミニゲームが対応する施設名
    protected bool isPlaying = false;

    // ミニゲーム開始
    public abstract void StartGame(Facility facility);

    // ミニゲーム終了
    public abstract void EndGame();

    // ゲーム進行用（必要ならUpdate的に呼ぶ）
    public abstract void Tick();

    // 施設強化の効果を適用
    public abstract void ApplyFacilityUpgrade(Facility facility);

    // プレイ中かどうか
    public bool IsPlaying()
    {
        return isPlaying;
    }

}