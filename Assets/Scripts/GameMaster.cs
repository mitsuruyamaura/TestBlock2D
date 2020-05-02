using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの進行を管理するクラス
/// </summary>
public class GameMaster : MonoBehaviour {

    [SerializeField, Header("ライフの最大値")]
    public int maxLife;
    [SerializeField, Header("ライフの閾(しきい)値")]
    public int threshold;
    [SerializeField, Header("ライフ用のバー")]
    public Slider lifeBar;

    [SerializeField, Header("現在のステージ数")]
    public int stageNum = 1;
    [SerializeField, Header("クリア目標のブロック数")]
    public int normaBlockNum;

    [SerializeField, Header("ステージの画像を設定用")]
    public SpriteRenderer mainVisual;

    [SerializeField, Header("ボール制御用クラス")]
    public BallController ballController;
    [SerializeField, Header("画面エフェクトの制御用クラス")]
    public FlushController flushController;
    [SerializeField, Header("ブロック生成用クラス")]
    public InitBlocks initBlocks;
    [SerializeField, Header("トランジション用クラス")]
    public TransitionManager transitionManager;

    [SerializeField, Header("デバッグ用スイッチ")]
    public bool debugOn;

    public static GAME_STATE gameState;   // ゲームの進行状態
  
    private string stageName = "stage_";  // ファイル名の固定値
    private int currentLife;              // ライフの現在値

    private bool isReset;                 // リスタート用スラグ
    public StageData stageData;

    void Awake(){
        // ゲームを止まっている状態にする
        gameState = GAME_STATE.STOP;

        // MenuシーンでのStageNumとStageDataのStageNumとを照合してステージを見つけ、ステージ毎の情報をセットする
        foreach (StageData.StageDataList data in stageData.stageDatas) {
            if (data.stageNum == SelectStage.stageNo) {
                stageNum = data.stageNum;
                initBlocks.fileName = data.stagefileName;
                initBlocks.startPos = data.startPosition;
                ballController.speed = data.ballSpeed;
            }
        }

        // Startメソッドが実行される前に必要な情報をセットする
        SetUp();
    }

    /// <summary>
    /// ゲーム進行に必要な情報をセット
    /// </summary>
    private void SetUp() {
        // ゲーム画面にステージに応じたメイン画像(隠れている画像)を表示
        mainVisual.sprite = Resources.Load<Sprite>("Textures/" + stageName + stageNum);

        // ライフバーを満タンにする(右側)
        lifeBar.value = 1;

        // 現在のライフ値を最大値にする
        currentLife = maxLife;
        
        if (!ballController.isInitPos) {
            // 初回ゲーム時には、ボールのスタート地点をリスタート用に登録
            ballController.InitStartPosition();
        } else {
            // リスタートしている場合には、ボールを登録してあるスタート地点へ戻す
            ballController.SetBallPosition();
        }

        if (!debugOn) {
            // デバッグ中でない場合、ブロックを並べる。
            // 並べ終わったら生成したブロック数が戻ってくるので、それをクリア目標数として設定
            // 右側の処理がメソッドの場合には、そのメソッドの結果(戻り値)が変数に入る
            normaBlockNum = initBlocks.CreateBlocks();
        } else {
            // デバッグ中の場合には、クリア目標はインスペクターで設定するのでブロック生成のみ行う
            initBlocks.CreateBlocks();
        }
        isReset = false;
    }

    /// <summary>
    /// ライフを減算する
    /// </summary>
    /// <param name="damage"></param>
    public void SubtractLife(int damage) {
        // damage分だけcurrentLifeより減らす
        currentLife -= damage;

        // ライフバーにcurrentLifeの値を反映する
        FetchLifeSlider();

        if (currentLife <= threshold) {
            // ライフの現在値が閾値よりも低下したら警告状態にする
            gameState = GAME_STATE.WARNING;
        }

        if (currentLife <= 0) {
            // currentLifeが0以下になったらゲームを終了する
            GameUp();
        }
    }

    /// <summary>
    /// ライフを加算する
    /// </summary>
    /// <param name="recover"></param>
    public void GainLife(int recover) {
        // recover分だけcurrentLifeを増やす
        currentLife += recover;

        // ライフバーにcurrentLifeの値を反映する
        FetchLifeSlider();

        if ((gameState == GAME_STATE.WARNING) && (currentLife > threshold)) {
            // 警告状態のときにライフが閾値よりも大きくなったらゲーム状態を通常のプレイ状態に戻す
            gameState = GAME_STATE.PLAY;           
        }
    }

    /// <summary>
    /// ライフバーにcurrentLifeの値を反映する
    /// </summary>
    private void FetchLifeSlider() {
        // currentLife,maxLifeはint型だが、valueの値はfloat型
        // そこで int => floatにキャスト(型変換)して計算する
        lifeBar.value = (float)currentLife / (float)maxLife;
    }

    /// <summary>
    /// ゲーム終了時の管理を行い、必要な各クラスへの処理を呼び出す
    /// </summary>
    public void GameUp() {
        gameState = GAME_STATE.GAME_OVER;
        isReset = false;

        // ボールを止める処理を呼び出す
        ballController.StopMoveBall();

        // 画面の点滅を止める処理を呼び出す
        flushController.CleanUpFlushEffect();

        // ゲーム終了の文字を表示
        transitionManager.infoText.enabled = true;
        transitionManager.infoText.text = "GAME OVER...\n\nTouch Screen To Retry!";
    }

    private void Update() {
        if (!isReset && (gameState == GAME_STATE.STOP)) {
            // ゲーム画面が表示されていない場合
            if (Input.GetMouseButtonDown(0)) {
                // ゲーム画面をタップするとトランジション処理が入り、ゲーム画面を表示する
                StartCoroutine(transitionManager.EnterScene());
            }
        }

        if (!isReset && (gameState == GAME_STATE.GAME_OVER)) {
            if (Input.GetMouseButtonDown(0)) {
                // リセット用フラグを立てて、２回以上ここに入らないようにする
                isReset = true;
                // 画面に残っているすべてのブロックを破棄する
                initBlocks.DestroyBlocks();

                // ゲーム画面を隠すトランジション処理
                StartCoroutine(transitionManager.ExitScene());

                // ゲームを初期化して、始められる準備をする
                SetUp();
            }
        }
    }
}
