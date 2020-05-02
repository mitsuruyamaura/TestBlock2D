﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/CreateStageData")]
public class StageData : ScriptableObject
{
    public List<StageDataList> stageDatas = new List<StageDataList>();

    [System.Serializable]
    public class StageDataList {         // ステージ毎の設定項目
        public int stageNum;             // ステージ番号
        public string stagefileName;     // ステージ用に参照する画像のファイル名
        public Vector3 startPosition;    // ブロックの生成のスタート位置
        public float ballSpeed;          // ボールの速度
        public float minLineLength;      // ラインの最小幅
        public float lineDuration;       // ラインの出現時間
        public int maxLife;              // ライフの最大値
        public int gameTime;             // ゲーム時間
    }
}
