using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面の色を変えたり点滅させるクラス
/// </summary>
public class FlushController : MonoBehaviour {

    [SerializeField, Header("点滅させるUIオブジェクト")]
    public Image img;
    [SerializeField, Header("点滅状態のフラグ")]
    public bool isFlush;

    private float timer;    // 経過時間計測用

    /// <summary>
    /// 画面の色を変える
    /// </summary>
    public void StartFlushEffect() {
        // ここでフラグが立つとUpdateの条件を満たすようになる
        isFlush = true;
    }

    void Update(){
        // フラグとゲームの状態に合わせて画面の色を変えたり、点滅させる
        
        // Bool型の場合、条件式にbool変数のみ書くとTrueかどうかを判定する
        if (isFlush) {
            // 画面を赤くする。引数はRGBAの並び順 Aはアルファ値
            img.color = new Color(0.5f, 0f, 0f, 0.5f);

            // もしもゲーム状態が警告状態なら画面を点滅させる
            if (GameMaster.gameState == GAME_STATE.WARNING) {
                timer += Time.deltaTime;
                if (timer > 1.0f) {
                    timer = 0;
                    isFlush = false;
                }
            } else {
                isFlush = false;
            }
        } else {
            // すぐ上のif文でisFlushがfalseになるので、上の条件後、こちらにも続けて入る
            // 赤を段々と薄くして透明に戻す
            // Color.Leapメソッドは第１引数と第２引数を第３引数に応じてミックスする
            // ここでは赤（第１）と透明（第２）に対して時間の経過によって赤→透明へと変化させている
            img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime);

            if (GameMaster.gameState == GAME_STATE.WARNING) {
                // もしもゲーム状態が警告状態なら画面を点滅させる
                timer += Time.deltaTime;
                if (timer > 1.5f) {
                    timer = 0;
                    StartFlushEffect();
                }
            }
        }
    }

    /// <summary>
    /// 画面の点滅を終了する
    /// </summary>
    public void CleanUpFlushEffect() {
        isFlush = false;
        // 画面の色のエフェクトを消して透明に戻す
        img.color = new Color(0f, 0f, 0f, 0f);
    }
}
