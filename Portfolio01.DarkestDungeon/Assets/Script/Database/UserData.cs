using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.DataBase
{
    /// <summary>
    /// 유저 데이터를 보관하는 역할을 할 클래스.
    /// </summary>
    [CreateAssetMenu(menuName = "Portfolio/UserData")]
    public class UserData : ScriptableObject
    {
        /// <summary>
        /// 플레이어 이름
        /// </summary>
        public string nickName;
        /// <summary>
        /// 플레이어 컨트롤러의 마지막 위치.
        /// </summary>
        public Vector3 lastPos;
        /// <summary>
        /// 플레이어가 마지막으로 갔던 스테이지 인덱스.
        /// </summary>
        public int lastStageIndex;
        /// <summary>
        /// 플레이어가 보유중인 영웅 목록.
        /// </summary>
        public List<GameObject> lastpossessionHero;
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
    }
}