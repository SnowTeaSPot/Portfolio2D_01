using Portfolio.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.UI
{
    /// <summary>
    /// UIWindow를 기반으로 생성될 모든 UI를 관리하는 매니저.
    /// </summary>
    public class UIWindowManager : Singleton<UIWindowManager>
    {
        /// <summary>
        /// 매니저에 등록된 UI 중 활성화 되어있는 UIWindow를 갖는 리스트
        /// </summary>
        private List<UIWindow> openWindows = new List<UIWindow>();
        /// <summary>
        /// 매니저에 등록된 모든 UIWindow를 갖는 리스트
        /// </summary>
        private List<UIWindow> totalUIWindows = new List<UIWindow>();
        /// <summary>
        /// 매니저에 등록된 모든 UIWindow를 갖는 딕셔너리.
        ///  -> 문자열로 특정 데이터를 탐색할 때 쓸 예정.
        /// </summary>
        private Dictionary<string, UIWindow> cachedUIWindows = new Dictionary<string, UIWindow>();
        /// <summary>
        /// 매니저에 등록된 UIWindow의 인스턴스에 접근 시, 해당 인스턴스들을 최종적으로 캐싱하여 담아둘 딕셔너리.
        ///  -> 내가 코드로 직접 접근해서 찾고자 하는 UIWindow 객체들만 담겨져 있음.
        /// </summary>
        private Dictionary<string, object> cachedInstances = new Dictionary<string, object>();

        /// <summary>
        /// UIWindowManager를 초기화할 때 실행할 함수.
        /// </summary>
        public void Initalize()
        {
            InitAllWindow();
        }

        private void Update()
        {
            // UIWindow는 esc키로 창을 종료할 수 있는지에 대한 bool 값을 가지고 있음.
            // esc 입력 시, 가장 최근에 활성화된(canCloseESC의 값이 true인) UIWindow를 닫아버림.
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                var target = GetTopWindow();

                if (target != null && target.canCloseESC)
                    target.Close();
            }
        }

        /// <summary>
        /// UIManager에 UIWindow 인스턴스를 등록하는 기능.
        /// </summary>
        /// <param name="uiWindow"></param>
        public void AddTotalWindow(UIWindow uiWindow)
        {
            // 딕셔너리에 등록 시, 자기 자신의 이름을 키 값으로 등록하기 위해 이름을 변수에 저장함.
            var key = uiWindow.GetType().Name;
            // 딕셔너리에 해당 키가 이미 등록되어 있는지 확인할 변수.
            bool hasKey = false;

            // 전체 UIWindow 리스트에 등록하고자 하는 인스턴스가 있는가?
            // 또는 전체 UIWindow 딕셔너리에 해당 인스턴스 타입의 키 값이 이미 존재 하는가??
            if (totalUIWindows.Contains(uiWindow) || cachedUIWindows.ContainsKey(key))
            {
                // 딕셔너리에 키 값으로 접근 시, 해당 값이 null이 아니라면 리턴.(이미 있다는 소리니까.)
                if (cachedUIWindows[key] != null)
                    return;
                // 키 값은 있는데 벨류가 null이라는 소리임.
                // 그럼 참조하고 있는 UIWindow의 인스턴스가 없다는 뜻.
                else
                {
                    hasKey = true;

                    // 한마디로 여기 오류가 났을 경우임.
                    // 이 경우에는 전체 UIWindow의 리스트를 순화하면서 null인 원소를 제거해주기.
                    for (int i = 0; i < totalUIWindows.Count; i++)
                    {
                        if (totalUIWindows[i] == null)
                            totalUIWindows.RemoveAt(i);
                    }
                }
            }

            // 이곳으로 들어왔을 경우, 해당 UIWindow는 매니저에 새롭게 등록을 해야 하는 경우임.
            // 즉, 전체 UIWindow 리스트, 딕셔너리에 등록 ㄱㄱ.

            totalUIWindows.Add(uiWindow);

            if (hasKey)
                cachedUIWindows[key] = uiWindow;
            else
                cachedUIWindows.Add(key, uiWindow);
        }

        /// <summary>
        /// 전체 활성화된 UIWindow 목록에서 활성화된 UI를 등록하는 기능.
        /// </summary>
        /// <param name="uiWindow">등록하려는 UIWindow</param>
        public void AddOpenWindow(UIWindow uiWindow)
        {
            // 리스트에 이미 존재하는지 한번 확인하기.
            if (!openWindows.Contains(uiWindow))
                // 리스트에 없다면 등록.
                openWindows.Add(uiWindow);
        }

        /// <summary>
        /// 전체 활성화된 UIWindow 목록에서 비활성화된 UI를 제거하는 기능.
        /// </summary>
        /// <param name="uiWindow">제거하고 싶은 UIWindow</param>
        public void RemoveOpenWindow(UIWindow uiWindow)
        {
            // 리스트에 이미 존재하는지 한번 확인하기.
            if(openWindows.Contains(uiWindow))
                // 리스트에 존재한다면 제거.
                openWindows.Remove(uiWindow);
        }

        /// <summary>
        /// 가장 마지막으로 열린 UI를 반환하는 기능.
        /// </summary>
        /// <returns></returns>
        public UIWindow GetTopWindow()
        {
            // 전체 활성화 UIWindow 목록에는 활성화된 순서대로 UI가 추가되기 때문에,
            // 목록의 가장 뒤쪽에서부터 인스턴스가 존재하는지 확인하는 거임.
            for(int i = openWindows.Count - 1; i > 0; i--)
            {
                // null이 아니라면??
                if (openWindows[i] != null)
                    // 존재한다는 뜻이므로 그대로 반환하기.
                    return openWindows[i];
            }

            // 이 과정을 거치고도 아무것도 없으면 걍 UI가 하나도 안열려 있다는 뜻이야.

            return null;
        }

        /// <summary>
        /// 매니저에 등록된 T타입의 UI를 반환하는 기능.
        /// </summary>
        /// <typeparam name="T">반환받고자 하는 UI의 타입.</typeparam>
        /// <returns>T타입의 UIWindow 인스턴스</returns>
        public T GetWindow<T>() where T : UIWindow
        {
            // 애초에 키를 해당 UI의 이름으로 등록해놨기 때문에 이렇게 해도 됨.
            string key = typeof(T).Name;

            if (!cachedUIWindows.ContainsKey(key))
                return null;

            // 최종적으로 T타입의 UIWindow 인스턴스는 인스턴스 딕셔너리를 통해 반환된다.
            // 그러면, 찾고자 하는 인스턴스가 인스턴스 딕셔너리에 존재하는지 검사한다.
            if (!cachedInstances.ContainsKey(key))
            {
                // 이 곳에 들어왔다는 것은, 등록되지 않았다는 뜻.
                // 따라서 전체 UIWindow 딕셔너리에서 해당 인스턴스를 가져와 T타입으로 캐스팅 후, 
                // 인스턴스 딕셔너리에 등록한다.
                cachedInstances.Add(key, (T)Convert.ChangeType(cachedUIWindows[key], typeof(T)));
            }
            // 인스턴스 딕셔너리에 키는 존재하지만, 이때 밸류도 존재하는 검사.
            else if (cachedInstances[key].Equals(null))
                // 밸류값이 없으면, 똑같이 인스턴스 딕셔너리에 등록하기.
                cachedInstances[key] = (T)Convert.ChangeType(cachedUIWindows[key], typeof(T));

            // 위의 과정을 통해, 최종적으로 인스턴스 딕셔너리에는 T타입 형태의 인스턴스가 저장된다.
            // 따라서, 특정 T타입의 인스턴스를 가져오는 기능을 호출 시, 매번 캐스팅하여 반환할 필요없이
            // 캐싱된 데이터를 사용하므로 편함. 어느정도 성능을 절약할 수 있다.
            //  -> 실제 성능상 큰 의미는 없고, 내가 직접 접근한 T타입의 인스턴스만 담긴다는 점에서
            //     인스턴스 접근에 대한 호출을 추적할 수 있다는게 큰 의미임.
            return (T)cachedInstances[key];
        }

        public void CloseAll()
        {
            for(int i = 0; i < totalUIWindows.Count; i++)
            {
                totalUIWindows[i]?.Close();
            }
        }

        /// <summary>
        /// 매니저에 등록된 모든 UI 초기화.
        /// </summary>
        public void InitAllWindow()
        {
            for(int i = 0; i < totalUIWindows.Count; i++)
            {
                totalUIWindows[i]?.InitWindow();
            }
        }
    }
}