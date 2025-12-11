using UnityEngine;
using System.Collections.Generic;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private List<MiniGame> miniGames; // 各パネルにアタッチ
    private MiniGame currentGame;

    public void StartMiniGame(Facility facility)
    {
        // 前回のパネルを非表示にする
        if (currentGame != null)
        {
            currentGame.gameObject.SetActive(false);
        }

        // 対応するミニゲームを検索
        MiniGame targetGame = miniGames.Find(g => g.FacilityName == facility.data.FacilityName.ToString());
        if (targetGame != null)
        {
            targetGame.gameObject.SetActive(true);
            targetGame.ApplyFacilityUpgrade(facility);

            if (!targetGame.IsPlaying())
            {
                targetGame.StartGame(facility);
            }

            currentGame = targetGame;
        }
    }

    private void Update()
    {
        currentGame?.Tick();
    }
}