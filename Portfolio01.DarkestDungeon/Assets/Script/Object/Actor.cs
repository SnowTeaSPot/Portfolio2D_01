using Portfolio.BO;
using Portfolio.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Object
{
    using State = Define.State;
    /// <summary>
    /// 인게임 내에서 다양한 행동을 하는 객체들의 추상화된 베이스격 클래스
    ///  ex) 캐릭터, 몬스터, 함정
    /// Actor의 파생클래스에서 공통되는 기능은 최대한 Actor에 정의해야 한다.
    /// 파생클래스에 따라 다른 기능은 해당 파생클래스에서 정의하기.
    /// </summary>
    public class Actor : MonoBehaviour, IPoolable
    {
        public int id;
        /// <summary>
        /// 애니메이터 상태 제어에 사용될 state 변수의 hash 값
        /// </summary>
        protected int animStateHash;

        /// <summary>
        /// 액터의 현재 상태.
        /// </summary>
        public State State { get; set; }
        public bool CanRecycle { get; set; } = true;

        /// <summary>
        /// 액터들의 bo 데이터 참조.
        /// </summary>
        public BoActor boActor;

        /// <summary>
        /// 액터가 공통적으로 사용하는 컴포넌트들의 참조
        /// </summary>
        protected Animator anim;

        /// <summary>
        /// 액터의 현재 행
        /// </summary>
        public int currentColumn;

        /// <summary>
        /// 초기화 시, 외부에서 boActor 데이터를 주입함.
        /// </summary>
        /// <param name="boActor"></param>
        public virtual void Initialize(BoActor boActor)
        {
            State = State.Idle;
            this.boActor = boActor;


        }

        protected virtual void Start()
        {
            // 액터들이 사용하는 컴포넌트 참조를 받기.
            anim = GetComponent<Animator>();

            animStateHash = Animator.StringToHash("state");

        }

        public virtual void SetStats() { }
        /// <summary>
        /// 액터들의 전용 업데이트
        /// </summary>
        public virtual void Execute()
        {
            MoveUpdate();
        }

        public virtual void MoveUpdate() { }

        public virtual void SetState(State state)
        {
            State = state;
            anim.SetInteger(animStateHash, (int)state);

            // 액터의 파생객체들의 공통적인 상태만을 베이스에서 처리함.
            // 그 후, 파생 객체에 따라 개별적으로 갖는 상태는 해당 파생클래스에서 처리.
            switch (state)
            {
                // TODO : 기능 구현 시, 아래에 채울 것.
                case State.Idle:
                    break;
                case State.Walk:
                    break;
                case State.Defend:
                    break;
                case State.Dead:
                    break;
            }
        }
    }
}
