using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 画面スワイプに合わせて線を引くクラス
/// </summary>
public class DrawSwipeLine : MonoBehaviour {

    [Header("描画する線のプレファブ")]
    public GameObject linePrefab;
    [Header("引く線の最小の長さ")]
    public float lineLength;
    [Header("引く線の太さ")]
    public float lineWidth;
    [Header("線の出現時間")]
    public float duration;

    private Vector3 touchPos;   // マウスのクリック地点
    private Rigidbody2D rb;

    public float accel;

    private void Update() {
        if (GameMaster.gameState == GAME_STATE.WAIT) {
            return;
        }
        if (GameMaster.gameState == GAME_STATE.GAME_OVER) {
            return;
        }
        // いつでも線が引ける状態にしておく
        DrawLine();
    }

    /// <summary>
    /// マウスクリックした地点からスワイプした位置まで線を引く
    /// </summary>
    private void DrawLine() {
//#if UNITY_EDITOR
//        if (EventSystem.current.IsPointerOverGameObject()) {
//            return;
//        }
//#else
//        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
//            return;
//        }
//#endif
        if (Input.GetMouseButtonDown(0)) {
            // マウスでクリックした位置を取得
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // 使わない情報は0にする
            touchPos.z = 0;
        }
        if (Input.GetMouseButton(0)) {
            // 最初にクリックした位置をスタート地点にする
            Vector3 startPos = touchPos;
            
            // クリックしている間、その位置を監視し、離した地点を最後の地点にする
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // 使わない情報は0にする
            endPos.z = 0;

            if ((endPos - startPos).magnitude > lineLength) {
                // マウスの動いた位置のベクトルの長さ(magnitude/float型)より、最小値よりも大きいなら線を生成
                GameObject obj = Instantiate(linePrefab, transform.position, transform.rotation) as GameObject;
                
                // 生成地点より、transformの現在値を更新して線にする
                obj.transform.position = (startPos + endPos) / 2;
                
                // 横方向に対して線を引く
                // ベクトルの方向を維持したまま、長さが1(正規化)のベクトルにする
                obj.transform.right = (endPos - startPos).normalized;

                // 線の太さを設定
                obj.transform.localScale = new Vector3((endPos - startPos).magnitude, lineWidth, lineWidth);

                // 線オブジェクトをこのクラスがアタッチされているオブジェクトの子にする
                obj.transform.parent = this.transform;

                // マウスの位置を更新
                touchPos = endPos;

                // 線を消す
                Destroy(obj, duration);
            }
        }
    }
}
