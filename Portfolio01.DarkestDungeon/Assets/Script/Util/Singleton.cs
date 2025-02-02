using UnityEngine;

namespace Portfolio.Util
{
    /// <summary>
    /// 싱글톤 패턴
    /// -> 객체의 인스턴스를 하나로 유지하는 방식.
    /// 
    /// 씬이 변경되어도 데이터가 유지되어야 하고, 외부에서 호출이 잦은 스크립트가 있다.
    /// 이럴 땐,여러 객체를 사용하는게 아니라, 단일 객체를 사용해야함.
    /// 위의 경우에 사용하는 것이 싱글톤이다.
    /// </summary>
    
    //이 싱글톤을 부모로 가지려면 T타입 명시를 꼭 해주어야함
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// 싱글톤에서 파생된 클래스의 인스턴스
        /// </summary>
        private static T instance = null;

        /// <summary>
        /// 외부에서 싱글톤 인스턴스에 접근하기 위한 읽기 전용 프로퍼티
        /// -> get에서 인스턴스가 존재하는지 확인할 것임
        /// </summary>
        public static T Instance
        {
            get
            {
                //인스턴스가 존재하는지 확인
                if(instance == null)
                {
                    // 인스턴스를 씬에서 찾음.
                    instance = FindObjectOfType<T>();
                    // 인스턴스를 찾았는지 확인.
                    if(instance == null )
                    {
                        // 없으면 게임 오브젝트를 생성.
                        GameObject obj = new GameObject(typeof(T).Name);
                        // 생성한 빈 객체에 T타입 컴포넌트를 붙인다.
                        instance = obj.AddComponent<T>();
                    }
                }

                // 결과.
                // 처음에 인스턴스가 존재할 경우, 바로 전환.
                // 존재하지 않는다면 찾거나, 생성하여 반환.
                // -> 이후에는 인스턴스가 null이 아니게 되므로 하나의 인스턴스만을 반환하게 된다.
                return instance;
            }
        }

        // Awake로 하는 이유.
        // Awake가 호출되었다는 것은 씬의 하이라키상에 해당 객체가 이미 존재한다는 뜻.
        protected virtual void Awake()
        {
            // 인스턴스가 Null인지 확인.
            if( instance == null )
            {
                // 인스턴스를 미리 넣어주는 작업을 실시.
                // -> 프로퍼티를 통해 접근 시, 객체를 찾거나 생성하는 과정을 생략
                instance = this as T;
                // 씬이 변경되어도 게임 오브젝트가 파괴되지 않도록 하는 함수.
                DontDestroyOnLoad(gameObject);
            }
            // 인스턴스가 있다면?
            else
            {
                // 이 시점에 인스턴스가 존재하는 것은 잘못된 사용임
                Destroy(gameObject);
            }
        }
    }
}

