using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.UI
{
    /// <summary>
    /// 모든 UI의 베이스 클래스. (팝업 제외)
    /// 이게 모든 UI의 엄마가 될거임.
    /// </summary>

    /// RequireComponent(특정할 컴포넌트) : 특정한 컴포넌트를 강제하는 기능.
    [RequireComponent(typeof(CanvasGroup))] // 이 스크립트 컴포넌트를 가지려면 Canvas Group 컴포넌트를 무조건 가지고 있어야 함.
    public class UIWindow : MonoBehaviour
    {
        /// <summary>
        /// 캔버스그룹을 참조하는 변수.
        /// </summary>
        private CanvasGroup canvasGroup;

        /// <summary>
        /// 캔버스 그룹 프로퍼티
        /// </summary>
        public CanvasGroup CanvasGroup
        {
            get
            {
                // 만약 캔버스그룹을 참조하는 필드에 아무것도 없다면??
                if (canvasGroup == null)
                    // 지금!! 당!! 장!! 투입시키기.
                    canvasGroup = GetComponent<CanvasGroup>();

                return canvasGroup;
            }
        }
        /// <summary>
        /// 해당 UI를 ESC버튼으로 닫을 수 있는가?
        /// </summary>
        public bool canCloseESC;
        /// <summary>
        /// 해당 UI가 활성화 상태인가?
        /// </summary>
        public bool isOpen;

        public virtual void Start()
        {
            InitWindow();
        }

        public virtual void InitWindow()
        {
            // UIManager에 등록.
            UIWindowManager.Instance.AddTotalWindow(this);

            // UIWindow를 기반으로 파생된 UI는 처음부터 비활성화된 상태로 존재할지라도 start에선 활성화 상태로 배치할 것임.

            // 왜요?
            // A : Start 함수가 제대로 실행되지 않기 때문이다.
            // 활성화 상태로 배치한 후, isOpen 값에 따라 비활성화 되어야 할 UI들을 자동적으로 비활성화 시킬 예정.

            if (isOpen)
            {
                Open(this);
            }
            else
            {
                Close(this);
            }
        }

        /// <summary>
        /// UI 활성화 기능.
        /// </summary>
        /// <param name="force">강제로 활성화 시킬 것인지????</param>
        public virtual void Open(bool force = false)
        {
            // 강제 활성화 여부를 확인.
            if (!isOpen || force)
            {
                isOpen = true;
                // 매니저의 활성화 UI 목록에 등록.
                UIWindowManager.Instance.AddOpenWindow(this);

                SetCanvasGroup(true);
            }
        }

        /// <summary>
        /// UI 비활성화 기능.
        /// </summary>
        /// <param name="force">강제로 비활성화 시킬 건지???????</param>
        public virtual void Close(bool force = false)
        {
            if (isOpen || force)
            {
                isOpen = false;
                // 매니저의 비활성화 UI 목록에 등록.
                UIWindowManager.Instance.RemoveOpenWindow(this);
                SetCanvasGroup(false);
            }
        }

        /// <summary>
        /// 캔버스그룹 내의 필드를 활성/비활성 여부에 따라 설정해주는 기능.
        /// (알아두면 굉장히 편리함.)
        /// </summary>
        /// <param name="isActive"></param>
        public void SetCanvasGroup(bool isActive)
        {
            // 해당 ui 요소 출력 시, 알파 값( 0 : 투명, 1 : 불투명)이을 정함.
            // isActive는 bool 변수인데, 이건 사실 bool이 아님.
            // 그래서 isActive 값에 따라서 int 0 또는 1의 값을 CanvasGroup.alpha에 대입하기 위함.
            // 투명도 조절.
            CanvasGroup.alpha = Convert.ToInt32(isActive);
            // 자식의 인터렉션.
            CanvasGroup.interactable = isActive;
            // 레이캐스팅 대상에 포함 여부.
            CanvasGroup.blocksRaycasts = isActive;
        }
    }
}