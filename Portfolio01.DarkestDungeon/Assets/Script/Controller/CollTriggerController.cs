using System;
using UnityEngine;

namespace Portfolio.Controller
{
    /// <summary>
    /// 프로젝트 내에서 트리거용으로 사용할 콜라이더를 갖는 객체들이 다수 존재한다.
    /// 다양한 종류의 객체들이 트리거용으로 사용하게 될텐데 그때마다 트리거 감지를 위해
    /// 새로운 클래스를 매번 생성하기 버겁기 때문에 만든 전용 클래스.
    /// 
    ///  -> 트리거 감지에 대한 기능을 일반화하여, 다양한 종류의 객체마다 트리거 감지에 따라.
    ///     실행시키고자 하는 기능을 대리자(Action)로 받아와서 처리함.
    ///     결과적으로, 해당 클래스 하나만을 정의하여 트리거를 사용하는 다양한 종류의 객체를 처리할 수 있음.
    ///     
    /// Action 연습용임.
    /// </summary>
    public class CollTriggerController : MonoBehaviour
    {
        /// <summary>
        /// 콜라이더가 겹치는 순간 실행시킬 기능을 대리할 대리자.
        /// </summary>
        private Action<Collider2D> enterEvent;

        /// <summary>
        /// 콜라이더가 겹치고 있는 동안 실행시킬 기능을 대리할 대리자.
        /// </summary>
        private Action<Collider2D> stayEvent;

        /// <summary>
        /// 콜라이더가 겹침이 끝나는 순간 실행시킬 기능을 대리할 대리자.
        /// </summary>
        private Action<Collider2D> exitEvent;

        /// <summary>
        /// 대리자의 대상이 되는 인스턴스의 콜라이더를 참조.
        /// </summary>
        public Collider2D coll;

        private void Start()
        {
            coll = GetComponent<Collider2D>();
        }

        public void Initialize(Action<Collider2D> onEnter = null, Action<Collider2D> onStay = null, Action<Collider2D> onExit = null)
        {
            enterEvent = onEnter;
            stayEvent = onStay;
            exitEvent = onExit;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 대리자가 존재할 경우 해당 기능을 실행.
            enterEvent?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            exitEvent?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            exitEvent?.Invoke(collision);
        }
    }
}