using Portfolio.UI;
using Portfolio.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Resource
{
    /// <summary>
    /// 실행 중에 필요한 리소스를 불러오는 기능을 담당할 클래스.
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        public void Initialize()
        {
            LoadAllPrefabs();
        }

        /// <summary>
        /// Assets/Resources 폴더 내의 프리팹을 불러와 변환하는 기능.
        /// </summary>
        /// <param name="path">Resources 폴더 내 불러올 에셋 경로</param>
        /// <returns></returns>
        public GameObject LoadObject(string path)
        {
            // Resources.Load -> Assets 폴더 Resources 라는 이름의 폴더가 존재한다면
            // 해당 경로로부터 path를 읽음.
            // 해당 경로에 파일이 GameObject 형태로 부를 수 있다면 불러온다.
            return Resources.Load<GameObject>(path);
        }

        /// <summary>
        /// 풀 매니저로 사용할 객체의 프리팹을 로드 후, 풀 매니저를 이용하여 풀을 등록하는 기능.
        /// </summary>
        /// <typeparam name="T">로드하고자 하는 타입.</typeparam>
        /// <param name="path">프리팹 경로.</param>
        /// <param name="poolCount">풀 등록 시, 초기 인스턴스 수.</param>
        /// <param name="loadComplete">로드 및 등록을 완료 후, 실행시킬 기능.</param>
        public void LoadPoolableObject<T>(string path, int poolCount = 1, Action loadComplete = null) where T : MonoBehaviour, IPoolable
        {
            // 프리팹을 로드하기.
            var obj = LoadObject(path);
            // 프리팹이 가지고 있는 T타입 컴포넌트 참조를 가져온다.(해당 클래스 가져오기)
            var tComponent = obj.GetComponent<T>();
            // T타입의 풀을 등록.
            PoolManager.Instance.RegistPool<T>(tComponent, poolCount);
            // 위 작업이 모두 끝난 후, 실행시킬 기능이 있다면? 같이 실행시키기.
            loadComplete?.Invoke();
        }

        /// <summary>
        /// 인게임에서 전반적으로 사용되는 프리팹들을 부르는 기능.
        ///  -> 주로 인게임 내에서 풀 매니저에 등록하여 사용할 프리팹들을 주로 써놓을 예정.
        /// </summary>
        public void LoadAllPrefabs()
        {
            LoadPoolableObject<HpBar>($"Prefabs/UI/Bar/HpBar", 10);
            LoadPoolableObject<StressBar>($"Prefabs/UI/Bar/StressBar", 10);
        }
    }
}