using Portfolio.Object;
using Portfolio.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Battle
{
    /// <summary>
    /// 다키스트 던전에 등장하는 상호작용 중, '전투'상호작용을 관리할 매니저.
    /// 상호작용이 생성될 때, 해당 전투에 등장할 적, 기습 성공 유무를 정함.
    /// 
    /// 플레이어 컨트롤러가 플레이어의 유닛을 들고 있는 것처럼, BattleManager도 적 유닛을 들고 있어야 한다.
    /// (한 번에 2개의 전투가 일어나지 않음. 연전이 일어난다고 쳐도 첫 번째 전투가 끝나야 함.)
    /// </summary>
    public class BattleManager : Singleton<BattleManager>
    {
        /// <summary>
        /// 플레이어가 전투중인 타일.
        /// </summary>
        public EnemyTile currentEnemyTile;
    }
}
