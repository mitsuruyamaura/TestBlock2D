using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "ScriptableObject/MakeBlockData")]
public class BlockData : ScriptableObject
{
    public List<BlockEachData> blockEachDatas = new List<BlockEachData>();

    [System.Serializable]
    public class BlockEachData {
        public Vector2 makePos;
        public int blockNo;
        public int score;
    }
}
