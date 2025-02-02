using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.SD
{
    /// <summary>
    /// EnemyTile의 SD데이터
    /// </summary>
    [SerializeField]
    public class SDEnemyTile : StaticData
    {
        /// <summary>
        /// 해당 조합이 등장할 수 있는 던전의 레벨.
        /// </summary>
        public int dungeonLevel;
        /// <summary>
        /// 조합에 들어간 적 유닛의 index.
        /// </summary>
        public int[] enemyArray;
        /// <summary>
        /// 해당 타일의 기습 확률.
        /// </summary>
        public int surprise;
    }
}
