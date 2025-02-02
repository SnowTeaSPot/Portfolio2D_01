using Portfolio.BO;
using Portfolio.Define;
using System;

namespace Portfolio.Object
{
    public class Enemy : Actor
    {
        /// <summary>
        /// Enemy의 bo데이터
        /// </summary>
        public BoEnemy boEnemy;
        /// <summary>
        /// Enemy의 종족.
        /// </summary>
        public Tribe tribe;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);

            boEnemy = boActor as BoEnemy;

            SetStats();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void SetStats()
        {
            base.SetStats();

            // 캐릭터 이름 지정.
            boEnemy.nickName = boEnemy.sdEnemy.nickName;

            // 기본 스탯 지정.
            boEnemy.type = Define.ObjectType.Enemy;
            boEnemy.currentLevel = boEnemy.sdEnemy.level;
            boEnemy.currentHp = boEnemy.maxHp = boEnemy.sdEnemy.maxHp;
            boEnemy.currentSpeed = boEnemy.sdEnemy.speed;
            boEnemy.currentEvade = boEnemy.sdEnemy.evade;
            boEnemy.currentDef = boEnemy.sdEnemy.defPer;
            boEnemy.curAccuracyCorrect = boEnemy.sdEnemy.accuracyCorrect;
            boEnemy.currentCritical = boEnemy.sdEnemy.critical;

            // Actor가 갖고 있는 각종 저항력 수치.
            // 모든 오브젝트가 갖고 있는 수치
            boEnemy.currentStunResist = boEnemy.sdEnemy.stunresist;
            boEnemy.currentAddictedResist = boEnemy.sdEnemy.addictedresist;
            boEnemy.currentBleedingResist = boEnemy.sdEnemy.bleedingresist;
            boEnemy.currentWeakeningResist = boEnemy.sdEnemy.weakeningResist;
            boEnemy.currentMovingResist = boEnemy.sdEnemy.movingresist;

            // 종족 지정.
            tribe = (Tribe)boEnemy.sdEnemy.tribe;
        }
        public override void SetState(State state)
        {
            base.SetState(state);
        }
    }
}
