using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    [SerializeField, Header("トランジション用マスク画像")]
    public Image maskImage;
    [SerializeField, Header("インフォテキスト表示用")]
    public TMP_Text infoText;

    private void Awake() {
        // ゲーム画面が見えないようにマスク画像を表示する
        maskImage.material.SetFloat("_Flip", maskImage.material.GetFloat("_Flip") + 2.0f);
        infoText.text = "2D BreakOut\n\nTouch Screen To Start!";
    }

    /// <summary>
    /// マスク画像を非表示にして、ゲーム画面を表示するトランジション処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnterScene() {
        // マスク画像をセットする
        // GetFloatしているのは現在値を取得し、そこに加算したいため
        // 値を直接入れればその値になる
        maskImage.material.SetFloat("_Flip", maskImage.material.GetFloat("_Flip") + 2.0f);
        yield return new WaitForSeconds(0.2f);

        // インフォ表示を非表示にする
        infoText.enabled = false;

        // マスク画像がゲーム画面から消えるまでアニメさせる
        while (maskImage.material.GetFloat("_Flip") > -1.0f) {
            // マテリアルのFlipプロパティを徐々に減算することでマスク画像を消す
            maskImage.material.SetFloat("_Flip", maskImage.material.GetFloat("_Flip") - 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        // ゲーム画面が表示されたらゲームの状態をREADY状態に変更する
        GameMaster.gameState = GAME_STATE.READY;
    }

    /// <summary>
    /// マスク画像を表示し、ゲーム画面を隠すトランジション処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExitScene() {
        // 一旦インフォ表示を非表示にする
        infoText.enabled = false;

        // マスク画像がゲーム画面を隠すまでアニメさせる
        while (maskImage.material.GetFloat("_Flip") < 1.0f) {
            // マテリアルのFlipプロパティを徐々に加算することでマスク画像を表示する
            maskImage.material.SetFloat("_Flip", maskImage.material.GetFloat("_Flip") + 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        // マスク画像が表示されたらインフォテキストを表示する
        infoText.enabled = true;
        infoText.text = "2D BreakOut\n\nTouch Screen To Start!";

        // ゲーム画面が隠れたらゲームの状態を停止状態にする
        GameMaster.gameState = GAME_STATE.STOP;
    }
}
