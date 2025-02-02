using Portfolio.Define;
using Portfolio.SD;
using System;
using UnityEngine;

namespace Portfolio.BO
{
    public class BoCharacter : BoActor
    {
        /// <summary>
        /// 현재 스트레스.
        /// </summary>
        
        public int currentStress;
        /// <summary>
        /// 현재 죽음의 저항 확률.
        /// </summary>
        public float currentDeathResist;

        /// <summary>
        /// 현재 질병 저항력.
        /// </summary>
        public float currentDiseaseResist;

        /// <summary>
        /// 현재 함정 해제 확률.
        /// </summary>
        public float currentTrapDisarm;
        
        /// <summary>
        /// 기질 배열.
        /// </summary>
        public Eccentricity[] ecentricity;

        /// <summary>
        /// BOActor에서는 sdActor 필드를 가지고 있고, sdActor의 형태로 되어있는 데이터를 접근이 편리하게
        /// 실제 캐릭터의 기획 데이터 형태로 캐스팅하여 담아둠.
        /// </summary>
        public SDCharacter sdCharacter;

        public BoCharacter(SDActor sdActor) : base(sdActor)
        {
            sdCharacter = sdActor as SDCharacter;
        }
    }
}
