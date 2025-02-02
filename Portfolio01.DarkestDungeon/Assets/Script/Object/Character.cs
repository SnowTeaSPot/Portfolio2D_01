using Portfolio.BO;
using Portfolio.Define;
using System.Collections.Generic;

namespace Portfolio.Object
{
    public class Character : Actor
    {
        public BoCharacter boCharacter;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);

            // 제어의 편리를 위해 모든 데이터를 베이스 형태로 관리하고 있기 때문에,
            // 데이터 접근의 편리함을 위해 파생 클래스 형태로 캐스팅하여 초기화 때 담아둔다.
            // BOCharacter로 접근하여도 boActor가 보임
            boCharacter = boActor as BoCharacter;

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
            boCharacter.nickName = boCharacter.sdCharacter.nickName;

            // 기본 스탯 지정.
            boCharacter.type = Define.ObjectType.Character;
            boCharacter.currentLevel = boCharacter.sdCharacter.level;
            boCharacter.currentHp = boCharacter.maxHp = boCharacter.sdCharacter.maxHp;
            boCharacter.currentSpeed = boCharacter.sdCharacter.speed;
            boCharacter.currentEvade = boCharacter.sdCharacter.evade;
            boCharacter.currentDef = boCharacter.sdCharacter.defPer;
            boCharacter.currentStress = 0;
            boCharacter.curAccuracyCorrect = boCharacter.sdCharacter.accuracyCorrect;
            boCharacter.currentMinATK = boCharacter.sdCharacter.minATK;
            boCharacter.currentMaxATK = boCharacter.sdCharacter.maxATK;
            boCharacter.currentCritical = boCharacter.sdCharacter.critical;

            // Actor가 갖고 있는 각종 저항력 수치.
            // 모든 오브젝트가 갖고 있는 수치
            boCharacter.currentStunResist = boCharacter.sdCharacter.stunresist;
            boCharacter.currentAddictedResist = boCharacter.sdCharacter.addictedresist;
            boCharacter.currentBleedingResist = boCharacter.sdCharacter.bleedingresist;
            boCharacter.currentMovingResist = boCharacter.sdCharacter.movingresist;

            // Character가 갖고 있는 각종 저항력 수치.
            // 변하는 수치
            boCharacter.currentDeathResist = boCharacter.sdCharacter.deathResist;
            boCharacter.currentDiseaseResist = boCharacter.sdCharacter.diseaseResist;
            boCharacter.currentTrapDisarm = boCharacter.sdCharacter.trapDisarm;
        }

        public override void SetState(State state)
        {
            base.SetState(state);

            switch (state)
            {
                case State.Skill0:
                    break;
                case State.Skill1:
                    break;
                case State.Skill2:
                    break;
                case State.Skill3:
                    break;
                case State.Camp:
                    break;
                case State.Heroic:
                    break;
                case State.Investigate:
                    break;
                case State.Afflicted:
                    break;
                case State.Combat:
                    break;
            }
        }
    }
}
