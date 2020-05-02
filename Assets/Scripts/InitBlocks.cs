using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画像なしのブロックを生成して指定位置に合わせて画像を入れ込み、画面に並べるクラス
/// </summary>
public class InitBlocks : MonoBehaviour
{
    [Header("ブロックの位置と画像の参照データ")]
    public BlockData blockData;
    [Header("ブロックの配置地点")]
    public Vector3 startPos;
    [Header("画像なしのブロックのプレファブ")]
    public GameObject planeBlock;
    [Header("ステージ用の画像ファイル名")]
    public string fileName = "version-difference";

    // ゲームオーバー時にブロックを破棄するためにリストに入れておく
    private List<GameObject> blocksList = new List<GameObject>();

    /// <summary>
    /// 画像なしのブロックを生成し、BlockDataを参照して位置と画像を入れ込む
    /// </summary>
    public int CreateBlocks(){
        // 設定用のすべてのスプライト(画像)を読み込んでおく
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/" + fileName);

        // 生成したブロックの数のカウント用
        int counter = 0;

        // BlockDataのリスト(BlockEachData)を１つずつ取得し、そのデータの持つ画像の番号と位置情報を入れ込む
        foreach (BlockData.BlockEachData data in blockData.blockEachDatas) {
            // 画像なしのブロックを、BlockDataの持つ位置情報の位置に生成する
            GameObject block = Instantiate(planeBlock, data.makePos, planeBlock.transform.rotation);

            // 取得しているすべての画像データから、BlockDataと同じ値を持つデータを見つけてBlockの画像にする
            block.GetComponent<SpriteRenderer>().sprite = System.Array.Find(sprites, (sprite) => sprite.name.Equals(fileName + "_" +data.blockNo));

            // BlockをBlock_Filedの子オブジェクトにする
            block.transform.parent = gameObject.transform;

            // Listにブロックを入れる
            blocksList.Add(block);

            // 生成が終わったので、カウントを１つ増やす
            counter++;
        }
        // すべてのブロックを生成したらスタート地点に画像を移動させる
        gameObject.transform.position = startPos;

        // 並べたブロックの数をGameMasterにフィードバックしてクリア目標数にする(戻り値を活用)
        return counter;
    }

    /// <summary>
    /// ゲーム画面にあるすべてのブロックを破棄する
    /// </summary>
    public void DestroyBlocks() {
        blocksList.Clear();
        gameObject.transform.position = Vector2.zero;
        blocksList = new List<GameObject>();
    }
}
