using UnityEngine;

/// <summary>
/// オブジェクトの破壊を行うクラス
/// </summary>
public class Destroyer : MonoBehaviour
{
    public int hp;
    public SpriteRenderer spriteRenderer;

    private void Start() {
        hp = 2;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Ball") {
            hp--;
            //hp -= col.gameObject.power;
            if (hp <= 0) {
                Debug.Log(hp);
                // GameMasterクラスをゲーム内で探してクリア目標数をデクリメント(-1)する
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().normaBlockNum--;
                Destroy(gameObject);
            } else {
                Debug.Log(hp);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D col) {
    //    if (col.gameObject.tag == "Ball") {
    //        // GameMasterクラスをゲーム内で探してクリア目標数をデクリメント(-1)する
    //        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().normaBlockNum--;
    //        Destroy(gameObject);
    //    }
    //}
}
