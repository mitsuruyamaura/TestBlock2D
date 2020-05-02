using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LifeManager : MonoBehaviour
{
    public int initLife;
    private int currentLife;
    private int missCount;

    public GameObject lifePrefab;
    public GameObject maskPrefab;

    private GameObject[] lifeObjects;
    private GameObject[] maskObjects;

    public GameMaster gameMaster;

    public int threshold;

    /// <summary>
    /// currentLife更新用のプロパティ
    /// </summary>
    public int LifePoint {
        
        // セッター
        set {
            // LifePointに代入された値がvalueに入っているので、それをcurrentLifeに代入する
            currentLife = value;

            // ライフイメージの更新
            // 残っているcurrentLife分だけライフのイメージを表示させる
            // 減少した分はマスクイメージを表示させる = ライフの位置が変わらないようにする
            for(int i = 0; i < initLife; i++){
                if (missCount <= i) {
                    lifeObjects[i].SetActive(true);
                    maskObjects[i].SetActive(false);
                } else {
                    lifeObjects[i].SetActive(false);
                    maskObjects[i].SetActive(true);
                }
            }
        }

        // ゲッター
        get {
            // currentLifeの値を更新する
            return currentLife;
        }
    }

    void Start() {
        // 画面にライフ用のイメージをライフの初期値分だけ表示するメソッド
        StartCoroutine(SetUpLifeObjects());
    }

    /// <summary>
    /// ライフ用のイメージをプレファブからライフの初期値分だけ生成する
    /// </summary>
    private IEnumerator SetUpLifeObjects() {
        // 配列の初期化。ライフの初期値分だけ配列の要素数を用意する
        lifeObjects = new GameObject[initLife];
        maskObjects = new GameObject[initLife];

        // 初期値分のライフイメージを生成
        for (int i = 0; i < initLife; i++) {
            
            // ライフのイメージをプレファブから生成
            lifeObjects[i] = Instantiate(lifePrefab, transform, true);

            // 生成されたイメージをこのクラスがコンポーネント化されているオブジェクトの子にする
            lifeObjects[i].transform.SetParent(gameObject.transform);

            // TODO アニメさせる

            // 徐々にライフを生成するために少し待機時間を作る
            yield return new WaitForSeconds(0.3f);

            // ライフが減少したときのマスクイメージをプレファブから生成
            maskObjects[i] = Instantiate(maskPrefab, transform , true);
            maskObjects[i].SetActive(false);

            // こちらも子にする
            maskObjects[i].transform.SetParent(gameObject.transform);
        }
        // プロパティにinitLifeを代入する = Setのvalueの値が代入される
        LifePoint = initLife;
    }

    /// <summary>
    /// ライフの減算処理
    /// </summary>
    /// <param name="attackPower"></param>
    public void ApplyDamage(int attackPower) {

        // ライフの現在値が0以上残っているなら
        if (currentLife > 0) {

            // ダメージが現在値よりも大きくなる(マイナスになる)なら
            if (attackPower > currentLife) {
                currentLife = 0;
            } else {
                // ライフの現在値を減算する
                currentLife -= attackPower;
                missCount += attackPower;
            }

            if (currentLife <= threshold) {
                // ライフの現在値が閾値よりも低下したら警告状態にする
                GameMaster.gameState = GAME_STATE.WARNING;
            }

            // ライフが残り0以下なら
            if (currentLife <= 0) {
                // GameMasterのステートをゲームオーバーに変更するメソッドを呼び出す
                gameMaster.GameUp();
            }
        }
        // LifePointプロパティを呼び出す
        //LifePoint = currentLife;
        UpdateDisplayLife(currentLife);
    }

    public void UpdateDisplayLife(int amountLife) {

        // ライフイメージの更新
        // 残っているcurrentLife分だけライフのイメージを表示させる
        // 減少した分はマスクイメージを表示させる = ライフの位置が変わらないようにする
        for (int i = 0; i < initLife; i++) {
            if (missCount <= i) {
                lifeObjects[i].SetActive(true);
                maskObjects[i].SetActive(false);
            } else {
                lifeObjects[i].SetActive(false);
                maskObjects[i].SetActive(true);
            }
        }
    }
}
