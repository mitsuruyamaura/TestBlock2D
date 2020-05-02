using System.Collections;
using UnityEngine;

/// <summary>
/// ボールを管理するクラス
/// </summary>
public class BallController : MonoBehaviour {

    [Header("ボールの速度")]
    public float speed;
    [SerializeField, Header("ダメージゾーン接触時のライフへのダメージ値")]
    public int lossLife;
    [SerializeField, Header("ボールのスタート地点登録用")]
    public bool isInitPos;
    
    public Rigidbody2D rb;        // コンポーネントの格納用

    private Vector2 direction;    // ボールの方向
    private Vector2 startPos;     // スタート地点

    /// <summary>
    /// ボールのスタート地点を登録
    /// </summary>
    public void InitStartPosition() {
        startPos = transform.position;

        // 初回のみスタート地点を登録をする。次回以降はこのメソッドが呼ばれないようにする
        isInitPos = true;
    }

    /// <summary>
    /// ボールをスタート地点へ戻す
    /// </summary>
    public void SetBallPosition() {
        transform.position = startPos;
    }

    void Update() {
        // TODO GameStateに合わせて処理を追加する

        if (GameMaster.gameState == GAME_STATE.READY) {
            // ゲームの進行状態がREADYの時のみ、画面をタップで止まっているボールを打ち出せる
            ShotBall();
        }
    }

    /// <summary>
    /// 角度を決めてボールを打ち出す
    /// </summary>
    public void ShotBall() {
        if (Input.GetMouseButtonDown(0)) {    // このInput処理は画面１タップと同じで扱える
            // ボールを打ち出す角度をランダムで決める
            // 角度によって速度が変化してしまうので、それを正規化して同じ速度にする
            direction = new Vector2(Random.Range(-2.5f, 2.5f), 1).normalized;

            // ボールを打ち出す(摩擦や空気抵抗、重力を切ってあるので、ずっと同じ速度で動き続ける)
            rb.velocity = direction * speed * transform.localScale.x;

            // ゲームの進行状態をPLAY状態にする = 以後タップをしても、この処理には来ないようにする
            GameMaster.gameState = GAME_STATE.PLAY;
        }
    }

    /// <summary>
    /// ボールを一時停止させてから再移動させる
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator BreakMoveBall(float waitTime) {
        // 現在のボールの速度ベクトルを記録する
        Vector2 breakDirection = rb.velocity;

        // ボールの速度ベクトルを0にする = 一時的にボールだけストップさせる
        rb.velocity = Vector2.zero;

        // 画面の揺れが収まるまで待つ
        yield return new WaitForSeconds(waitTime);
        
        if (GameMaster.gameState != GAME_STATE.GAME_OVER) {
            // ゲームオーバー状態でないなら、ボールに速度ベクトルを戻し、再移動させる
            rb.velocity = breakDirection;
        }
    }

    /// <summary>
    /// ボールを止める
    /// </summary>
    public void StopMoveBall() {
        // ボールの速度ベクトルを0にして止める
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            Vector2 dir = transform.position - col.gameObject.transform.position;
            rb.velocity = dir * speed * Random.Range(1.0f,2.0f) * transform.localScale.x;
            Debug.Log(Random.Range(1.0f, 2.0f));
        }
    }
}
