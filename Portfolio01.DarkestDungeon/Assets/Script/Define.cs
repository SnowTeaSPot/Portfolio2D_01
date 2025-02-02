namespace Portfolio.Define
{
    /// <summary>
    /// 종족값 Enum
    /// </summary>
    public enum Tribe
    {
        corpse,     // 시체.
        human,      // 인간.
        unHoly,     // 불경.
        beast,      // 야수.
        eldritch,   // 이물.
        stone,      // 석재(가고일).
    }

    public enum SkillType
    {
        /// <summary>
        /// 없음(괴인 전용)
        /// </summary>
        None,
        /// <summary>
        /// 근접
        /// </summary>
        Melee,
        /// <summary>
        /// 원거리
        /// </summary>
        Ranged,
        /// <summary>
        /// 치료
        /// </summary>
        Care,
        /// <summary>
        /// 버프
        /// </summary>
        Buff,
    }
    public enum StageType
    {
        /// <summary>
        /// 영지
        /// </summary>
        Wisdom = 1000,
        /// <summary>
        /// 던전 선택
        /// </summary>
        Dungeon_Selection,
        /// <summary>
        /// 원정
        /// </summary>
        Expedition_Readiness,
        /// <summary>
        /// 폐허. 여기부터 던전 라인.
        /// </summary>
        Dungeon,
        /// <summary>
        /// 전투중.
        /// </summary>
        Battle,
    }

    /// <summary>
    /// 대상 지정 타입.
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 지정 가능한 대상이 없음.
        /// (아군, 적군 지정이 불가능한 자기 강화류 기술들.)
        /// </summary>
        None,
        /// <summary>
        /// 지정 대상 단일 타겟.
        /// </summary>
        singleTarget,
        /// <summary>
        /// 지정한 행의 모든 대상을 타겟.
        /// </summary>
        areaTarget,
    }

    /// <summary>
    /// 타일의 타입 분류.
    /// </summary>
    public enum TileType
    {
        /// <summary>
        /// 통로 타일.
        /// </summary>
        Passage,
        /// <summary>
        /// 방 타일.
        /// </summary>
        Room,

    }

    public enum SceneType
    {
        Title,
        Ingame,
        Loading,
    }

    public enum IntroPhase
    {
        Start,
        AppSetting,
        StaticData,
        UserData,
        Resource,
        UI,
        Complete,
    }

    public enum Eccentricity
    {
        /// <summary>
        /// 기벽 없음
        /// </summary>
        None,
        /// <summary>
        /// 재빠른 반사신경 : 속도 + 2
        /// </summary>
        QuickReflexes,
        /// <summary>
        /// 돌팔이 의사 : 회복 기술 - 20%
        /// </summary>
        BadHealer,
        /// <summary>
        /// 폐허 전문가 : 폐허 던전에서 피해량 + 15%
        /// </summary>
        RuinsExpert,
        /// <summary>
        /// 빛의 전사 : 횃불이 70% 이상일 때 추가 데미지 + 20%
        /// </summary>
        LightWarrior,
    }

    public class AllBuff
    {
        /// <summary>
        /// 약화 계열 상태이상을 따로 분류하기.
        /// </summary>
        public enum AddictedStatus
        {
            /// <summary>
            /// 없음. (약화에 걸리지 않았음.)
            /// </summary>
            None,
            /// <summary>
            /// 공격력
            /// </summary>
            Atk,
            /// <summary>
            /// 속도
            /// </summary>
            Speed,
            /// <summary>
            /// 명중률
            /// </summary>
            Accuracy,
            /// <summary>
            /// 회피
            /// </summary>
            Evade,
            /// <summary>
            /// 방어력
            /// </summary>
            Def,
            /// <summary>
            /// 치명타 확률
            /// </summary>
            Crit,
            /// <summary>
            /// 스트레스
            /// </summary>
            Stress,
            /// <summary>
            /// 각종 저항력.
            /// </summary>
            AddictedResist,
            BleedResist,
            WeakeningResist,
            MovingResist,
        }
        /// <summary>
        /// 상태이상.
        /// </summary>
        public enum AbnormalStatus
        {
            /// <summary>
            /// 없음. (약화에 걸리지 않았음.)
            /// </summary>
            None,
            /// <summary>
            /// 약화
            /// </summary>
            Addicted,
            /// <summary>
            /// 출혈
            /// </summary>
            Bleeding,
            /// <summary>
            /// 질병
            /// </summary>
            Disease,
            /// <summary>
            /// 공격력
            /// </summary>
            Atk,
            /// <summary>
            /// 속도
            /// </summary>
            Speed,
            /// <summary>
            /// 명중률
            /// </summary>
            Accuracy,
            /// <summary>
            /// 회피
            /// </summary>
            Evade,
            /// <summary>
            /// 방어력
            /// </summary>
            Def,
            /// <summary>
            /// 치명타 확률
            /// </summary>
            Crit,
            /// <summary>
            /// 스트레스
            /// </summary>
            Stress,
            /// <summary>
            /// 각종 저항력.
            /// </summary>
            AddictedResist,
            BleedResist,
            WeakeningResist,
            MovingResist,
        }

        /// <summary>
        /// 계수가 들어있지 않은 특수 상태이상을 분류한 Enum
        /// </summary>
        public enum SpecialStatus
        {
            None,
            /// <summary>
            /// 기절
            /// </summary>
            Stun,
            /// <summary>
            /// 표적
            /// </summary>
            Mark,
            /// <summary>
            /// 보호
            /// </summary>
            Guard,
            /// <summary>
            /// 반격
            /// </summary>
            Riposte,
            /// <summary>
            /// 죽음의 문턱
            /// </summary>
            DeathDoor,
            /// <summary>
            /// 은신
            /// </summary>
            Stealth,
            /// <summary>
            /// 은신 해제.
            /// </summary>
            DeStealth,
        }
    }

    public enum ObjectType
    {
        Character = 6,
        Enemy,
        Ground,
        Door,
        Accessible,
        Trap,
    }

    /// <summary>
    /// 액터의 상태.
    /// </summary>
    public enum State
    {
        Skill0,
        Skill1,
        Skill2,
        Skill3,
        Idle,
        Walk,
        Defend,
        Dead,
        Camp,
        Heroic,
        Investigate,
        Afflicted,
        Combat,
    }

    public class StaticData
    {
        // 변수 바뀌지 말라고 const 사용
        public const string SDPath = "Assets/Resources/StaticData";
        public const string SDExcelPath = "Assets/Resources/StaticData/Excel";
        public const string SDJsonPath = "Assets/Resources/StaticData/Json";
    }

    public class UI
    {
        public enum DungeonRightSide
        {
            Map,
            Inventory,
        }
    }
}