using Portfolio.SD;
using UnityEngine;

namespace Portfolio.BO
{
    /// <summary>
    /// 기획데이터에 사용되는 데이터를 프로그램에서 바로 사용하는 것이 아니라.
    /// 데이터를 프로그램에 사용하기 좋은 형태로 한 번 가공한다.
    ///  -> 인게임 로직 등에서 데이터를 사용하기 편한 형태로 가공한다.
    /// </summary>
    public class BoActor
    {
        /// <summary>
        /// 액터의 이름.
        /// </summary>
        public string nickName;
        /// <summary>
        /// 현재 최대 체력.
        /// </summary>
        public int maxHp;
        /// <summary>
        /// 현재 체력.
        /// </summary>
        public int currentHp;
        /// <summary>
        /// 현재 레벨.
        /// </summary>
        public int currentLevel;
        /// <summary>
        /// 현재 최소 공격력.
        /// </summary>
        public int currentMinATK;
        /// <summary>
        /// 현재 최대 공격력.
        /// </summary>
        public int currentMaxATK;
        /// <summary>
        /// 현재 속도.
        /// </summary>
        public int currentSpeed;
        /// <summary>
        /// 현재 회피율.
        /// </summary>
        public int currentEvade;
        /// <summary>
        /// 현재 치명타 확률.
        /// </summary>
        public float currentCritical;
        /// <summary>
        /// 현재 명중 보정.
        /// </summary>
        public float curAccuracyCorrect;
        /// <summary>
        /// 현재 방어율.
        /// </summary>
        public float currentDef;
        /// <summary>
        /// 캐릭터 던전 이동속도.
        /// </summary>
        public float movespeed;

        /// <summary>
        /// 현재 기절 저항력
        /// </summary>
        public float currentStunResist;

        /// <summary>
        /// 현재 중독 저항력
        /// </summary>
        public float currentAddictedResist;

        /// <summary>
        /// 현재 출혈 저항력
        /// </summary>
        public float currentBleedingResist;

        /// <summary>
        /// 현재 약화 저항력
        /// </summary>
        public float currentWeakeningResist;

        /// <summary>
        /// 현재 이동 저항력
        /// </summary>
        public float currentMovingResist;


        /// <summary>
        /// 캐릭터 이동가능 여부.
        /// </summary>
        public bool canMove = true;
        /// <summary>
        /// 캐릭터 공격가능 여부.
        /// </summary>
        public bool canAttack = true;
        /// <summary>
        /// 캐릭터 생존여부.
        /// </summary>
        public bool canLive = true;

        /// <summary>
        /// 액터의 기획 데이터.
        /// </summary>
        public SDActor sdActor;

        // 액터의 타입.
        public Define.ObjectType type;

        /// <summary>
        /// 생성자에서 기획 데이터를 넘김.
        /// </summary>
        /// <param name="sdActor"></param>
        public BoActor(SDActor sdActor)
        {
            this.sdActor = sdActor;
        }

    }
}
