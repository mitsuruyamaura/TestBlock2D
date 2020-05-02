using UnityEngine;
using DG.Tweening;

/// <summary>
/// オブジェクトを揺らすクラス
/// </summary>
public class ObjectShaker : MonoBehaviour {

    [SerializeField, Header("揺らすオブジェクト")]
    public Transform shakeObj;
    [SerializeField, Header("揺らす時間")]
    public float duration;

    [SerializeField, Header("画面エフェクトの制御用クラス")]
    public FlushController flushController;
    [SerializeField, Header("ボール制御用クラス")]
    public BallController ballController;
    [SerializeField, Header("ゲーム管理用クラス")]
    public GameMaster gameMaster;

    public LifeManager lifeManager;

    private Transform orijinTran;            // オブジェクトの位置記録用

    private void Start() {
        // 揺らす前にオブジェクトの現在の位置を記録しておく
        orijinTran = shakeObj.transform;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Ball") {
            // 別のクラスにある、画面を点滅させるメソッドを呼び出す
            flushController.StartFlushEffect();

            // オブジェクトを揺らす
            Shake();

            // Ballを一時的に止める=コルーチンを使う
            // 別のクラスのコルーチンを呼び出す場合には、引数の中に変数をすべて入れる
            StartCoroutine(ballController.BreakMoveBall(duration));

            // Lifeを減らす
            //gameMaster.SubtractLife(ballController.lossLife);
            lifeManager.ApplyDamage(1);
        }
    }

    /// <summary>
    /// 対象のオブジェクトを揺らす
    /// </summary>
    public void Shake() {
        // DOTweenのシークエンス(動きを組み立てる)機能の初期化(利用可能状態にする)
        Sequence seq = DOTween.Sequence();

        // オブジェクトを揺らす
        seq.Append(shakeObj.DOShakePosition(duration));

        // オブジェクトを元の位置に戻す
        seq.Append(shakeObj.DOMove(orijinTran.position, 0.25f));
    }
}
