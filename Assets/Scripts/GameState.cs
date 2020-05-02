/// <summary>
/// ゲームの進行状態の種類の設定クラス
/// </summary>
public enum GAME_STATE {
    STOP,        // ゲームが動いていない状態
    READY,       // ゲーム開始の準備ができた状態。ボールを打ち出せる
    PLAY,        // ゲーム中の状態
    WARNING,     // 警告状態。ライフが閾（しきい）値より低下するとこの状態になる
    GAME_OVER,   // ゲームオーバー状態
    WAIT,        // 待機状態。一時停止など
    COUNT        // カウント用補助
}
