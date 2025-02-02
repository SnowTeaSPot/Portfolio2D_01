using System;

namespace Portfolio.SD
{
    public class SDActor : StaticData
    {
        /// <summary>
        /// 액터의 이름.
        /// </summary>
        public string nickName;
        /// <summary>
        /// 최대 체력
        /// </summary>
        public int maxHp;
        /// <summary>
        /// 리소스 경로.
        /// </summary>
        public string resourcePath;
        /// <summary>
        /// 동작 속도
        /// </summary>
        public int speed;
        /// <summary>
        /// 회피치
        /// </summary>
        public int evade;
        /// <summary>
        /// 방어율
        /// </summary>
        public float defPer;
        /// <summary>
        /// 명중 보정.
        /// </summary>
        public float accuracyCorrect;
        /// <summary>
        /// 치명타 확률
        /// </summary>
        public float critical;
        /// <summary>
        /// 기절 저항력
        /// </summary>
        public float stunresist;
        /// <summary>
        /// 중독 저항력
        /// </summary>
        public float addictedresist;
        /// <summary>
        /// 출혈 저항력
        /// </summary>
        public float bleedingresist;
        /// <summary>
        /// 약화 저항력.
        /// </summary>
        public float weakeningResist;
        /// <summary>
        /// 이동 저항력
        /// </summary>
        public float movingresist;
        /// <summary>
        /// 액터 레벨.
        /// </summary>
        public int level;
    }
}
