using Portfolio.Define;
using Portfolio.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Skill
{
    using AbnormalStatus = Define.AllBuff.AbnormalStatus;
    using AddictedStatus = Define.AllBuff.AddictedStatus;
    using SpecialStatus = Define.AllBuff.SpecialStatus;
    /// <summary>
    /// 모든 아군, 적군 캐릭터가 사용할 스킬의 기반이 될 클래스.
    /// 캐릭터에게 보유 스킬 컬렉션을 추가하고 스킬 슬릇(4칸)에 필요한 스킬을 장착하기.
    /// </summary>
    public class Skill : MonoBehaviour
    {
        /// <summary>
        /// 스킬의 이름.
        /// </summary>
        public string skillName;
        /// <summary>
        /// 해당 스킬의 레벨.
        /// </summary>
        public int skillLevel;
        /// <summary>
        /// 해당 스킬의 타입.
        /// </summary>
        public SkillType skillType;
        /// <summary>
        /// 스킬 시전자.
        /// TODO : 스킬을 장착할 경우 해당 변수에 자기 자신을 등록해야 함.
        /// </summary>
        public Actor skillCaster;
        /// <summary>
        /// 해당 스킬을 사용 가능한 행.
        /// </summary>
        public int[] availableColumn;
        /// <summary>
        /// 지정 가능한 아군 대상 행.
        /// </summary>
        public int[] friendlyTarget;
        /// <summary>
        /// 지정 가능한 적 대상 행.
        /// </summary>
        public int[] enemyTarget;

        /// <summary>
        /// 스킬의 성능을 보관하는 변수와 그의 프로퍼티.
        /// </summary>
        [SerializeField]
        private SkillList skillList = new SkillList();
        public SkillList SkillList { get { return skillList; } }
    }

    /// <summary>
    /// 스킬을 담을 리스트를 가질 클래스.
    /// </summary>
    [Serializable]
    public class SkillList
    {
        /// <summary>
        /// 스킬 리스트
        /// </summary>
        public List<AttackSkillGuide> attackSkill;
        public List<CareSkillGuide> careSkill;
        public List<BuffSkillGuide> buffSkill;
        public List<SpecialSkillGuide> specialSkill;

        /// <summary>
        /// 생성자에서 해당 변수에 새로운 스킬을 추가함.
        /// 클래스 안의 리스트를 보이게 하려면 이렇게 생성자에서 만들어줘야.
        /// </summary>
        public SkillList()
        {
            attackSkill = new List<AttackSkillGuide>();
            careSkill = new List<CareSkillGuide>();
            buffSkill = new List<BuffSkillGuide>();
            specialSkill = new List<SpecialSkillGuide>();
        }
    }

    /// <summary>
    /// 모든 스킬 가이드의 기본 설정을 넣을 부모 클래스.
    /// </summary>
    public class SkillGuide
    {
        
    }

    /// <summary>
    /// 공격 스킬이 가져야 할 기본 세팅.
    /// </summary>
    [Serializable]
    public class AttackSkillGuide
    {
        /// <summary>
        /// 치명타 보정
        /// </summary>
        public float cRITCorrect;
        /// <summary>
        /// 명중률.
        /// 해당 공격의 기본 명중률.
        /// </summary>
        public float accuracy;
        /// <summary>
        /// 공격력 보정.
        /// </summary>
        public float atkCorrect;
        /// <summary>
        /// 대상 지정 방식.
        /// 단일 타겟인지 지역 타겟인지 여부.
        /// </summary>
        public TargetType targetType;
    }

    /// <summary>
    /// 치료 스킬이 가져야 할 기본 세팅.
    /// </summary>
    [Serializable]
    public class CareSkillGuide
    {
        /// <summary>
        /// 치명타 보정.
        /// </summary>
        public float cRITCorrect;
        /// <summary>
        /// 최소 회복량.
        /// </summary>
        public int minCareIndex;
        /// <summary>
        /// 최대 회복량.
        /// </summary>
        public int maxCareIndex;
        /// <summary>
        /// 대상 지정 방식.
        /// </summary>
        public TargetType targetType;
    }

    /// <summary>
    /// 버프 스킬이 가져야 할 기본 세팅.
    /// </summary>
    [Serializable]
    public class BuffSkillGuide
    {
        /// <summary>
        /// 자기 자신을 대상으로 한 버프.
        /// </summary>
        public List<Buff> casterTargetBuff;
        /// <summary>
        /// 아군을 대상으로 한 버프.
        /// </summary>
        public List<Buff> friendlyTargetBuff;
        /// <summary>
        /// 적을 대상으로 한 버프.
        /// </summary>
        public List<Buff> enemyTargetBuff;

        public BuffSkillGuide()
        {
            casterTargetBuff = new List<Buff>();
            friendlyTargetBuff = new List<Buff>();
            enemyTargetBuff = new List<Buff>();
        }


        [Serializable]
        public class Buff
        {
            /// <summary>
            /// 적용할 상태이상.
            /// </summary>
            public AbnormalStatus abnormalStatus;
            /// <summary>
            /// 적용할 약화.
            /// </summary>
            public AddictedStatus addictedStatus;
            /// <summary>
            /// 적용할 버프의 계수.
            /// </summary>
            public float buffIndex;
            /// <summary>
            /// 해당 버프의 유지 턴 수.
            /// </summary>
            public int turnIndex;
        }
    }

    /// <summary>
    /// 특수 스킬이 가져야 할 기본 세팅.
    /// </summary>
    [Serializable]
    public class SpecialSkillGuide
    {
        /// <summary>
        /// 자기 자신을 대상으로 한 버프.
        /// </summary>
        public List<Buff> casterTargetBuff;
        /// <summary>
        /// 아군을 대상으로 한 버프.
        /// </summary>
        public List<Buff> friendlyTargetBuff;
        /// <summary>
        /// 적을 대상으로 한 버프.
        /// </summary>
        public List<Buff> enemyTargetBuff;

        public SpecialSkillGuide()
        {
            casterTargetBuff = new List<Buff>();
            friendlyTargetBuff = new List<Buff>();
            enemyTargetBuff = new List<Buff>();
        }


        [Serializable]
        public class Buff
        {
            /// <summary>
            /// 적용할 특수 상태이상.
            /// </summary>
            public SpecialStatus specialstatus;
            /// <summary>
            /// 해당 버프의 유지 턴 수.
            /// </summary>
            public int turnIndex;
        }
    }
}