using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Portfolio.Util
{
    /// <summary>
    /// 풀 오브젝트를 형성하고 관리할 매니저.
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        public Dictionary<string, object> poolDict = new Dictionary<string, object>();

        public void Initailize()
        {
            // 풀 매니저 초기화 시, 딕셔너리에 있는 내용을 전부 없애버림.
            poolDict.Clear();
        }

        /// <summary>
        /// Pool 클래스
        /// 풀링 가능한 오브젝트는 무조건 IPoolable 인터페이스와 MonoBehaviour를 부모로 가지고 있어야 한다.
        /// 
        /// 왜냐하면, MonoBehaviour를 가지고 있지 않을 경우 유니티 하이라키 상에서 관리가 안되기 때문이다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Pool<T> where T : MonoBehaviour, IPoolable
        {
            /// <summary>
            /// T 타입 객체를 갖는 풀
            /// 읽기 전용 프로퍼티
            /// </summary>
            public List<T> PoolObject {  get; private set; } = new List<T>();

            /// <summary>
            /// 해당 풀의 객체들의 인스턴스를 하이라키 상에서 들고 있을 부모 객체의 트랜스폼
            /// (하이라키 상에서 관리하기 쉽게 정리용으로 사용할 홀더)
            /// </summary>
            public Transform holder;

            /// <summary>
            /// 풀에서 재사용이 가능한 객체가 존재하는지를 나타내는 프로퍼티
            ///  Pool.Find(_ => _.CanRecycle)
            ///  풀 안에 있는 객체 중 CanRecucle 같이 true인 객체가 있는지 찾는 작업.
            ///  풀 안에 있는 T 타입 객체는 IPoolable을 상속받으므로 CanRecycle 프로퍼티를 갖음.
            /// </summary>
            public bool CanRecycle => PoolObject.Find(_ => _.CanRecycle) != null;

            /// <summary>
            /// 풀링할 새로운 T 타입 인스턴스를 등록
            /// </summary>
            /// <param name="obj">풀에 넣고자하는 T 타입 </param>
            public void Regist(T obj)
            {
                PoolObject.Add(obj);
            }

            /// <summary>
            /// 풀에서 꺼내서 사용하던 객체를 풀에 다시 반환하는 기능.
            /// (정확히는 객체는 항상 풀에 존재하고, 풀에서 객체를 꺼낸다고 표현하고 있지만.
            ///  실제로 이루어지는 작업은, 재사용 가능한 객체를 찾아서 활성/비활성화 && 객체의 부모를 초기화하는 것임)
            ///  
            /// 풀에 있을 경우 : 객체의 부모는 오브젝트 풀의 홀더 필드
            /// 풀에서 꺼냈을 경우 : 상황에 따라 내가 지정한 다른 부모를 가짐.
            /// </summary>
            /// <param name="obj"></param>
            public void Return(T obj)
            {
                // 반환했기 때문에 객체의 부모를 다시 홀더로 지정.
                obj.transform.SetParent(holder);
                obj.gameObject.SetActive(false);
                // 반환했으니까 재사용 가능.
                obj.CanRecycle = true;
            }

            /// <summary>
            /// 풀 내의 재사용 가능하고 특정 조건을 만족하는 객체를 반환
            /// T를 condition에 넣으며
            /// 조건은 없어도 되기에 null로 일단은 지정 
            /// </summary>
            /// <param name="condition">내가 찾고자 하는 특정 조건, condition은 true 혹은 false가 될 것임</param>
            /// <returns>재사용 가능하고 특정 조건을 만족하는 T 타입 인스턴스</returns>
            public T GetObject(Func<T, bool> condition = null)
            {
                // 풀 내에서 재사용 가능한 객체가 없다면?
                if (!CanRecycle)
                {
                    // 조건이 없다면 풀에서 아무 객체나 하나 가져오기.
                    // 조건이 있다면 조건과 동일한 객체를 하나 가져오기.
                    var tmpObj = condition != null ? PoolObject.Find(_ => condition(_)) : PoolObject[0];

                    // 재사용이 현재는 불가능하지만, 조건을 만족하는 객체가 있다면?
                    if (tmpObj != null)
                    {
                        var clone = Instantiate(tmpObj.gameObject, holder);
                        //A(clone)으로 나오는 것을 A로 바꾸는 작업 
                        clone.name = tmpObj.name;
                        clone.SetActive(false);
                        // 새로 생성한 객체를 풀에 등록하기.
                        Regist(clone.GetComponent<T>());
                    }
                    else
                    {
                        Debug.Log($"{typeof(T).Name}이(가) 모두 떨어져서 추가 생성을 해야 합니다.");
                        return null;
                    }
                }

                // 풀에 재사용 가능한 객체가 존재하거나, 존재하지는 않지만 조건을 만족하는 객체를 찾아서 복사했다면
                // 여기로 들어오게 된다.

                T recycleObj = condition != null ? PoolObject.Find(_ => condition(_) && _.CanRecycle) : PoolObject.Find(_ => _.CanRecycle);

                // 재사용 가능한 객체가 없다면??
                if (recycleObj == null)
                {
                    Debug.Log($"{typeof(T).Name}가 다 떨어짐.");

                    //var tmpObj = condition != null ? PoolObject.Find(_ => condition(_)) : PoolObject[0];

                    //// 등록 만들고 여기 넣어야 함.

                    //tmpObj.CanRecycle = false;

                    return null;
                }

                // 여기까지 왔다면 재사용할 객체를 찾았다는 뜻이기 때문에
                // 해당 객체를 이제 사용할 것이므로 재사용 프로퍼티를 더 이상 사용할 수 없게 변경한다
                recycleObj.CanRecycle = false;
                return recycleObj;

            }
        }

        public void RegistPool<T>(T obj, int poolCount = 1) where T : MonoBehaviour, IPoolable
        {
            Pool<T> pool = null;

            // T타입 이름을 딕셔너리의 키 값으로 활용하기.

            var key = typeof(T).Name;

            // 딕셔너리에 이미 키가 존재한다면??
            if (poolDict.ContainsKey(key))
            {
                // 기존에 존재하던 풀 클래스를 사용하기
                //  이미 풀이 존재한다는 뜻이고, 해당 기능은 풀을 새로 등록하는 기능이다.
                //  등록을 중단하지 않고, 기존의 풀을 그대로 받아오기.
                //
                //  현재 제작중인 오브젝트 풀은 동일한 T 타입이라도
                //  실제 인스턴스 시에 모델이나 세부 로직이 다른 인스턴스도 담을 수 있게 하려고 함.
                //   ex) 몬스터 풀이 존재하고 그 풀에 a라는 몬스터를 등록했다고 치자.
                //       이후 b라는 몬스터를 풀링하여 사용하고자 할 때, 굳이 새로운 풀을 만들 필요 없이
                //       기존에 있던 동일한 몬스터 풀을 이용할 수 있게 하려고 한다.
                pool = poolDict[key] as Pool<T>;
            }
            // 존재하지 않는다면
            else
            {
                // 해당 키 값의 풀이 등록되어있지 않으므로, 새로 생성하여 딕셔너리에 추가하기.
                pool = new Pool<T>();
                poolDict.Add(key, pool);
            }

            // 풀에 홀더가 존재하는지 체크하여, 홀더가 없다면 생성하기.
            if(pool.holder == null)
            {
                // 홀더의 이름을 T 타입 Holder로 지정하여 생성하겠다.
                pool.holder = new GameObject($"{key}Holder").transform;
                // 홀더의 부모 객체를 오브젝트 풀 매니저로 지정
                //  결과, 오브젝트 풀 매니저는 싱글톤 객체이고 현재 싱글톤 객체는
                //  DontDestroyOnLoad로 설정되어 있어서 씬이 변경되어도 파괴되지 않는다.
                //  따라서, 오브젝트 풀에 있는 객체들도 파괴되지 않음
                //  이 말은 상황에 따라 적절히 풀이 필요 없어질 때 수동적으로 해제해야 한다.
                pool.holder.parent = transform;
                pool.holder.position = Vector3.zero;
            }

            // 파라미터로 받은 풀에 초기 생성할 인스턴스 수 만큼 오브젝트 생성.
            for(int i = 0; i < poolCount; i++)
            {
                // 인스턴스를 생성하여 풀에 등록하기.
                var poolableObject = Instantiate(obj);
                poolableObject.name = obj.name;
                // 생성한 인스턴스의 부모를 holder로 설정
                poolableObject.transform.SetParent(pool.holder);
                poolableObject.gameObject.SetActive(false);

                // 초기 작업이 끝난 인스턴스를 풀에 등록.
                pool.Regist(poolableObject);
            }
        }
        /// <summary>
        /// Pool<T>는 반환 값
        /// GetPool<T>는 함수 이름
        /// poolDict에 등록된 특정 풀을 찾아서 반환
        /// </summary>
        /// <typeparam name="T">찾고자 하는 풀의 타입(MonoBehaviour 혹은 IPoolable)</typeparam>
        /// <returns>T 타입의 풀</returns>
        public Pool<T> GetPool<T>() where T : MonoBehaviour, IPoolable
        {
            var key = typeof(T).Name;

            // 딕셔너리에 해당 타입의 key값이 존재하는지 검사
            if(!poolDict.ContainsKey(key))
            {
                return null;
            }

            // 키 값이 존재한다면, 해당 키 값을 가진 poolDict를 반환하기.
            return poolDict[key] as Pool<T>;
        }

        /// <summary>
        /// 특정 풀을 비워버리는 기능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearPool<T>() where T : MonoBehaviour, IPoolable
        {
            // 해당 기능은 특정 씬에서 들고있을 필요가 없는 풀이 발생했을 때
            // 해당 풀을 비우기 위한 용도로 사용.

            var pool = GetPool<T>()?.PoolObject;

            // 풀이 없다면 리턴
            if (pool == null)
                return;

            // 있다면 풀 안에 있는 풀러브 객체를 전부 파괴한다.
            for(int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] != null)
                    // 게임오브젝트 삭제
                    Destroy(pool[i].gameObject);
            }

            // 리스트 내에서 삭제
            pool.Clear();
        }
    }
}