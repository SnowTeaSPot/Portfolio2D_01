using Portfolio.DataBase;
using Portfolio.DataBase.Handler;
using Portfolio.Define;
using Portfolio.Resource;
using Portfolio.UI;
using System.Collections;
using UnityEngine;

namespace Portfolio
{
    /// <summary>
    /// 타이틀 씬에서 게임 시작 전에 필요한 전반적인 초기화를 수행할 클래스.
    /// 추가로 데이터 로드를 수행함.
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        /// <summary>
        /// 현재 페이즈의 완료 상태.
        /// </summary>
        private bool loadComplete;

        /// <summary>
        /// 외부에서 loadComplete에 접근하기 위한 프로퍼티.
        /// 현재 페이즈를 완료할 시 조건에 따라 다음 페이즈로 변경.
        /// </summary>
        public bool LoadComplete
        {
            get { return loadComplete; }
            set 
            {
                loadComplete = value;
                // 현재 페이즈가 완료되었고, 모든 페이즈가 완료되지 않았다면 다음 페이즈로 변경해야 함.
                if(loadComplete && !AllLoaded)
                //다음 페이즈로 변경.
                    NextPhase();
            }
        }

        /// <summary>
        /// 모든 페이즈의 완료 상태
        /// </summary>
        public bool AllLoaded { get; set; }
        
        /// <summary>
        /// 현재 페이즈를 나타내는 변수.
        /// </summary>
        private IntroPhase introPhase = IntroPhase.Start;

        /// <summary>
        /// 로딩 게이지 애니메이션을 처리할 때 사용할 코루틴 객체를 담아둘 변수.
        ///  -> 페이즈가 빠르게 변경될 시, 이전에 발생된 코루틴이 존재하는 상태에서
        ///     동일한 코루틴이 다시 발생시킬 시 문제가 발생하기 때문임.
        ///     따라서, 이 때 이전에 발생된 코루틴 객체를 담아두기 위해 사용.
        /// </summary>
        private Coroutine loadGaugeUpdateCoroutine;

        public UITitle uiTitle;

        /// <summary>
        /// 타이틀 컨트롤러 초기화.
        /// </summary>
        public void Initialize()
        {
            OnPhase(introPhase);
        }

        /// <summary>
        /// 페이즈를 다음으로 변경
        /// </summary>
        private void NextPhase()
        {
            StartCoroutine(WaitForSeconds());

            IEnumerator WaitForSeconds()
            {
                yield return new WaitForSeconds(.5f);
                
                loadComplete = false;

                OnPhase(++introPhase);
            }
        }

        /// <summary>
        /// 현재 페이즈에 대한 로직 실행
        /// </summary>
        /// <param name="phase">진행시키고자 하는 현재 페이즈</param>
        private void OnPhase(IntroPhase phase)
        {
            OnPhaseAnimation(phase);

            // TODO : 기능을 만들 때마다, 여기다가 추가해줘야 함.
            switch (phase)
            {
                case IntroPhase.Start:
                    
                    break;
                case IntroPhase.AppSetting:
                    // 싱글톤 사용법.
                    GameManager.Instance.OnAppSetting();
                    break;
                case IntroPhase.StaticData:
                    GameManager.SD.Initialize();
                    break;
                case IntroPhase.UserData:
                    DummyServer.Instance.Initialiaze();

                    new LoginHandler();
                    break;
                case IntroPhase.Resource:
                    ResourceManager.Instance.Initialize();
                    break;
                case IntroPhase.UI:
                    UIWindowManager.Instance.Initalize();
                    break;
                case IntroPhase.Complete:
                    var stageManager = StageManager.Instance;

                    GameManager.Instance.LoadScene(SceneType.Ingame, stageManager.ChangeStage(DummyServer.Instance.userData.lastStageIndex),
                        stageManager.OnChangeStageComplete);
                    AllLoaded = true;
                    break;
            }
            LoadComplete = true;
        }

        private void OnPhaseAnimation(IntroPhase phase)
        {
            // 현재 페이즈를 문자열로 전달하여 로딩 상태 텍스트를 갱신.
            uiTitle.SetState(phase.ToString());

            // 이전에 로딩 게이지 업데이트 코루틴을 발생시켰을 때에
            // 아직 로딩게이지 UI의 fillAmount가 실제 로딩 게이지 %만큼
            // 보간이 되어있지 않다면, 아직 코루틴을 실행중이라는 뜻이다.

            // 이미 실행중인 코루틴을 또 발생시키면 오류가 발생하기 때문에
            // 코루틴이 존재한다면 멈춘 후에, 새로 변경된 % 값을 넘겨 코루틴을 새로 시작하자.
            if (loadGaugeUpdateCoroutine != null)
            {
                StopCoroutine(loadGaugeUpdateCoroutine);
                loadGaugeUpdateCoroutine = null;
            }

            //변경된 페이즈가 전체 페이즈 완료가 아니라면
            if (phase != IntroPhase.Complete)
            {
                // 현재 로드 % 값을 구하기.
                var loadPer = (float)phase / (float)IntroPhase.Complete;
                // 구한 %를 로딩바에 적용.
                loadGaugeUpdateCoroutine = StartCoroutine(uiTitle.LoadGaugeUpdate(loadPer));
            }
            // 완료하면 강제로 uitillAmount를 1로 변경.
            else
                uiTitle.loadGauge.fillAmount = 1f;
        }

        
    }
}

