using Portfolio.Bo;
using Portfolio.Define;
using Portfolio.SD;
using Portfolio.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Portfolio
{
    /// <summary>
    /// 게임에 사용하는 모든
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// 씬 전환 시, 현재 로딩 상태를 나타내는 함수.
        /// 0 ~ 1로 사용
        /// </summary>
        public float loadState;

        // 유저 데이터
        [SerializeField]
        private BoUser boUser = new BoUser();
        public static BoUser User => Instance.boUser;

        // 기획 데이터.
        [SerializeField]
        private StaticDataModule sd = new StaticDataModule();

        public static StaticDataModule SD { get { return Instance.sd; } }

        /// <summary>
        /// 현재 스테이지 상태.
        /// </summary>
        public StageType currentStage;
        protected override void Awake()
        {
            base.Awake();

            // 타이틀 씬에서 페이즈 로드를 실행하기 위해
            // 타이틀 컨트롤러 참조를 찾기.
            var titleController = FindObjectOfType<TitleController>();
            // 참조를 찾았다면, 초기화 실행.
            titleController?.Initialize();
        }

        /// <summary>
        /// 앱 기본 설정.
        /// </summary>
        public void OnAppSetting()
        {
            // 수직 동기화 끄기.
            QualitySettings.vSyncCount = 0;
            //렌더 프레임을 60으로 설정
            Application.targetFrameRate = 60;
            //장시간 입력 없어도 화면이 꺼지지 않음.
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        /// <summary>
        /// 씬을 비동기로 로드하는 기능.
        /// 다른 씬 간의 전환에 사용할 것이다.
        /// (ex Title -> Ingame)
        /// </summary>
        /// <param name="type">로드할 씬의 이름</param>
        /// <param name="loadProgress">씬 전환 시 먼저 처리할 작업</param>
        /// <param name="loadComplete">씬 전환 완료 후, 실행할 기능</param>
        public void LoadScene(SceneType type, IEnumerator loadProgress = null, Action loadComplete = null)
        {
            StartCoroutine(WaitForLoad());

            // 씬을 전환할 때 한 번에 전환하는 것이 아니라
            // 중간에 로딩 씬을 넣어서 최종적으로 Title -> Loading -> Ingame 순으로 넘어가게 해줌.

            // 코루틴 -> 유니티에서 특정 작업을 비동기로 실행할 수 있게 해주는 기능.
            //           실제로 비동기는 아님, 실행 타임을 별개로 둬서 비동기처럼 보이는 것.


            //LoadScene 메서드에서 사용 가능한 코루틴 함수 선언
            IEnumerator WaitForLoad()
            {
                // 로딩 상태를 0으로 초기화.
                loadState = 0;

                // 비동기로 현재 씬에서 로딩 씬으로 전환
                //  -> 씬 전환 시, 화면이 멈추지 않게 하기 위해서
                yield return SceneManager.LoadSceneAsync(SceneType.Loading.ToString());

                // 변경하려고 하는 씬을 추가. (현재 게임에 씬이 2개가 된 상태임)
                //  -> LoadScene함수를 써서 씬을 변경할 경우, 2번째 매개변수를 사용하지 않는다면 기본 값이 적용됨.
                //    기본 값은 현재 씬(로딩) 비활성화하고 변경하고자 하는 씬을 활성화 함.
                //    하지만, 현재 하고자 하는 작업은 그대로 두고 새로운 씬을 추가하는 작업.
                //    이때, 씬 로드 방식을 Additive로 설정하면 된다.
                var asyncOper = SceneManager.LoadSceneAsync(type.ToString(), LoadSceneMode.Additive);

                // 결과적으로 현재 게임에는 2개의 씬이 활성화된 상태 (로딩 씬, 변경하고자 하는 씬)
                // 따라서, 원치 않은 2개의 씬의 객체들이 모두 랜더링 되기 때문에, 당장 필요없는 변경하려고 하는 씬을 비활성화 하는 것.
                // -> 씬은 그대로 두고 하이라키 상에 존재하는 것을 비활성화만 시킴.
                asyncOper.allowSceneActivation = false;

                // 변경하고자 하는 씬에 필요한 작업이 존재한다면?
                if(loadProgress != null)
                    // 해당 작업이 완료될 때까지 대기
                    yield return StartCoroutine(loadProgress);
                
                // 위의 작업이 완료된 후에 아래 로직이 시작되야 함.
                //  -> 변경하고자 하는 씬에 필요한 작업이 모두 완료됐다는 뜻이기 때문이다.

                // 로딩바에 진행 상태를 변경하여 유저에게 로딩 상태를 알림
                // 추가로, 변경하고자 하는 씬이 로드가 완료된 상태인지를 확인해야 한다.
                //  -> 비동기로 로드하고, 로드 시 yield return을 하지 않았기 때문에 어느 시점에 완료될 지 알 수 없음.

                // 비동기로 로드한 씬이 로드가 완료되지 않았다면, 특정 작업을 반복.
                while(!asyncOper.isDone)
                {
                    // loadState 값을 사용해서 유저에게 상태를 전달.
                    if(loadState >= .9f)
                    {
                        // 90% 이상 완료됐다면 강제로 100%로 만듦.
                        //  왜냐하면 asyncOper의 progress 값은 정확하게 1이 들어오지 않음.
                        loadState = 1f;

                        // 로딩바가 차는 것을 확인하기 위해 .5초 정도 대기.
                        yield return new WaitForSeconds(.5f);

                        // 변경하고자 하는 씬을 다시 활성화
                        //  (isDone은 씬이 활성 상태가 아니면, 로드가 완료되었더라도 true가 되지 않기 때문이다.)
                        asyncOper.allowSceneActivation = true;
                    }
                    else
                    {
                        // asyncOper에 현재 로드 진행 상태를 0부터 1의 값으로 나타내는 프로퍼티가 있다.
                        // 해당 값을 loadState에 대입
                        loadState = asyncOper.progress;
                    }

                    yield return null;
                }
                yield return new WaitForSeconds(.5f);

                // 로딩 씬에서 변경하고자 하는 씬에 필요한 작업을 전부 수행했으니
                // 로딩 씬을 비활성화 시킴
                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());

                // 모든 작업이 완료되었으므로, 추가적으로 실행할 작업이 존재한다면 그것까지 실행.
                // loadComplete의 값이 null이 아닌 경우 Invoke 실행
                loadComplete?.Invoke();
            }
        }
    
        /// <summary>
        /// 실제 씬을 변경하는 것이 아니라, 로딩 씬을 추가하여 스테이지 전환에 필요한 작업을 실시.
        /// 인게임 씬에서 스테이지 전환 시 사용할 함수임.
        /// ex) 인게임 씬(영지) -> 인게임 씬(던전 입장 지역)
        /// </summary>
        /// <param name="loadProgress"></param>
        /// <param name="loadComplete"></param>
        public void OnAdditiveLoadingScene(IEnumerator loadProgress = null, Action loadComplete = null)
        {
            StartCoroutine(WaitForLoad());

            IEnumerator WaitForLoad()
            {
                loadState = 0;

                var asyncOper = SceneManager.LoadSceneAsync(SceneType.Loading.ToString(), LoadSceneMode.Additive);

                // TODO : UILoading 만들면 여기다 추가하기.

                yield return null;

                loadState = .3f;
                
                if(loadProgress != null)
                    yield return StartCoroutine(loadProgress);

                loadState = .8f;

                yield return new WaitForSeconds(.5f);

                loadState = 1f;

                // 작업이 전부 완료되었다면 아래 함수를 실행할 것.
                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());

                loadComplete?.Invoke();
            }
        }
    }
}
