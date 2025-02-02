using Portfolio.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Portfolio.SD
{
    [CreateAssetMenu(menuName = "Portfolio/CharacterData")]
    public class SDCharacter : SDActor
    {
        /// <summary>
        /// 액터의 직업
        /// </summary>
        public string job;
        /// <summary>
        /// 최대 스트레스
        /// </summary>
        public int maxStress;
        /// <summary>
        /// 배고픔 게이지
        /// </summary>
        public int hungry;
        /// <summary>
        /// 죽음의 저항 확률
        /// </summary>
        public float deathResist;
        /// <summary>
        /// 질병 저항력
        /// </summary>
        public float diseaseResist;
        /// <summary>
        /// 함정 해제 확률
        /// </summary>
        public float trapDisarm;
        /// <summary>
        /// 최소 공격력
        /// </summary>
        public int minATK;
        /// <summary>
        /// 최대 공격력
        /// </summary>
        public int maxATK;

    }
}
