using Portfolio.DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.BO
{
    /// <summary>
    /// UserData는 저장용 SD임.
    /// 실제 인게임에선 BoAccount의 데이터를 사용해서 이동함.
    /// 
    /// 그 후, 세이브 할때마다 BoAccount의 데이터를 UserData로 옮겨야 함.
    ///  (게임 종료 후, UserData에 저장할 데이터를 담아둘 수 있다.)
    /// </summary>
#if UNITY_EDITOR
    [Serializable]
    // 유니티 에디터에서만 해당 코드가 작동하도록 조정하는 에디터 함수.
#endif
    public class BoAccount
    {
        public string nickName;

        public int currentStageIndex;

        public List<GameObject> currentPosseionHero;

        // 현재 보유중인 각종 재화들.
        /// <summary>
        /// 흉상(재화)
        /// </summary>
        public int bust;
        /// <summary>
        /// 초상화(재화)
        /// </summary>
        public int portrait;
        /// <summary>
        /// 증서(재화)
        /// </summary>
        public int bond;
        /// <summary>
        /// 문장(재화)
        /// </summary>
        public int emblem;
        /// <summary>
        /// 파편(재화) (미구현 요소)
        /// </summary>
        public int fragment;
        /// <summary>
        /// 금화
        /// </summary>
        public int gold;

        /// <summary>
        /// 게임을 진행하는 BoAccount에 있는 정보를 사용하고,
        /// 게임을 종료하거나 중간 세이브가 진행될 때마다 BoAccount의 내용을 유저 DB로 저장하기.
        /// </summary>
        /// <param name="userdata"></param>
        public BoAccount(UserData userdata)
        {
            nickName = userdata.nickName;
            currentStageIndex = userdata.lastStageIndex;
            currentPosseionHero = userdata.lastpossessionHero;
            bust = userdata.bust;
            portrait = userdata.portrait;
            bond = userdata.bond;
            emblem = userdata.emblem;   
            fragment = userdata.fragment;
            gold = userdata.gold;
        }
    }
}
