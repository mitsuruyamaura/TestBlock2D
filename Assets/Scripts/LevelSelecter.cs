using UnityEngine;

/// <summary>
/// ゲームの難易度を設定・管理するクラス
/// </summary>
public class LevelSelecter : MonoBehaviour{

    [SerializeField, Header("難易度の設定値")]
    public GAME_LEVEL settingLevel;

    public static GAME_LEVEL selectGameLevel;    // 選択中の難易度

    /// <summary>
    /// ゲームの難易度を設定
    /// </summary>
    /// <param name="level"></param>
    public void SelectLevel(GAME_LEVEL level) {
        selectGameLevel = level;
    }
}
