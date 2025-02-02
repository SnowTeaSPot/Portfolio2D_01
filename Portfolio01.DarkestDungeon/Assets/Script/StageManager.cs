using Portfolio.Define;
using Portfolio.Object;
using Portfolio.Resource;
using Portfolio.UI;
using Portfolio.Util;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Portfolio
{
    /// <summary>
    /// 스테이지 관련 기능들을 제어할 클래스
    /// 주로 스테이지 전환 시 처리작업 (리소스 로드 및 인스턴스 생성)
    /// </summary>
    public class StageManager : Singleton<StageManager>
    {
        /// <summary>
        /// 스테이지 전환 시, 스테이지 전환에 필요한 모든 작업이 완료된 상태인지를 나타내는 필드.
        /// </summary>
        private bool isReady;
        /// <summary>
        /// 현재 스테이지의 인스턴스
        /// </summary>
        private GameObject currentStage;

        private void Update()
        {
            if (!isReady)
                return;
        }

        /// <summary>
        /// 스테이지 전환 시, 필요한 리소스를 불러오고 인스턴스 생성 및 데이터 바인딩 작업.
        ///  -> 이 메서드를 호출하는 시점은 로딩 씬이 활성화 되어있는 상태임.
        /// </summary>
        /// <param name="index">바꿀 스테이지의 index</param>
        /// <returns></returns>
        public IEnumerator ChangeStage(int index)
        {
            isReady = false;
            
            // 매개변수를 참고해서 바꿀 스테이지의 SD데이터를 담아두는 변수 생성.
            var sdStage = GameManager.SD.sdstages.Where(_=>_.index == index).SingleOrDefault();
            var resourceManager = ResourceManager.Instance;

            // 현재 스테이지가 null이 아니라면.(이미 있는 상태라면??)
            if(currentStage != null)
                // 현재 스테이지를 파괴.
                Destroy(currentStage);

            // SD데이터의 프리팹경로를 통해 새로운 스테이지 생성.
            //  -> 문제가 하나 생기는데, 해당 객체를 생성하는 시점은 로딩 씬이 활성화 되어있고
            //     변경하고자 하는 씬은 비활성화 되어있는 상태임.
            //     이때 객체를 생성 시, 생성되는 객체는 활성화된 씬에 종속됨.
            //     따라서, 최종적으로 인게임 씬으로 전환되었을 때 스테이지가 보이지 않는다.(로딩 씬에서 스테이지가 생성됐기 때문에.)
            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath));

            // 위의 문제를 해결하고자 생성한 객체를 로딩 씬에서 변경하고자 하는 씬으로 이동시킴.
            SceneManager.MoveGameObjectToScene(currentStage, SceneManager.GetSceneByName(SceneType.Ingame.ToString()));

            // TODO : 이외에도 각종 NPC나 UI같은걸 클리어 해야 한다면, 여기서 전부 초기화 시키기.
            // 이전 스테이지가 '던전' 스테이지일 경우도 있으니 Enemy관련 인스턴스를 전부 클리어 함.
            PoolManager.Instance.ClearPool<Enemy>();
            // 이전 스테이지에서 사용한 인게임 UI요소 비우기.
            UIWindowManager.Instance.GetWindow<UIIngame>()?.Clear();

            // 현재 스테이지 Index를 변경.
            GameManager.User.boAccount.currentStageIndex = index;

            // 현재 스테이지 상태를 변경.
            GameManager.Instance.currentStage = sdStage.stageType;

            yield return null;
        }

        /// <summary>
        /// ChangeStage 메서드가 씬 전환 도중에 실행되는 작업이라면,
        /// OnChangeStageComplete()는 씬 전환이 완료된 후에 실행될 작업이다.
        ///  ex) 플레이어 배치, 오브젝트 배치.
        /// </summary>
        public void OnChangeStageComplete()
        {
            // 인게임 씬에서 플레이어 컨트롤러 참조를 찾기.
            var playerController = FindObjectOfType<PlayerController>();
            var cameraController = FindObjectOfType<CameraController>();
            // 맵을 변경할 때마다, 현재 출전 목록을 갱신해야 함.
            playerController.Initialize();
            // 플레이어 데이터를 스테이지 전환을 완료할 때마다 저장해주기. (자동 저장 같은 거임.)
            playerController.PlayerDataSave();
            // 스테이지 전환이 끝난 후 키 설정 변경.
            playerController.KeyReigstration();
            // 플레이어 컨트롤러의 초지션을 해당 스테이지의 0, 0, 0으로 변경.(모든 스테이지의 시작 위치는 월드 좌표 0, 0, 0임)
            playerController.transform.position = Vector3.zero;
            var uiWindowManager = UIWindowManager.Instance;
            // 모든 UI 초기화.
            UIWindowManager.Instance.CloseAll();

            // 현재 스테이지의 상태에 따라 띄워야 하는 UI를 정함.
            switch (GameManager.Instance.currentStage)
            {
                case StageType.Wisdom:
                    break;
                case StageType.Dungeon_Selection:
                    break;
                case StageType.Expedition_Readiness:
                    break;
                case StageType.Dungeon:
                    uiWindowManager.GetWindow<UIDungeon>().Open();
                    uiWindowManager.GetWindow<UIInventory>().Open();
                    uiWindowManager.GetWindow<UIEquipment>().Open();
                    break;
            }

            // 카메라 컨트롤러의 초기화 기능.
            cameraController.Initialize();

            isReady = true;
        }

        /// <summary>
        /// 플레이어 캐릭터 생성 또는 스테이지 이동 시 플레이어 위치 설정
        /// </summary>
        /// <param name="playerController">하이라키에 존재하는 플레이어 컨트롤러 참조</param>
        private void SpawnCharacter(PlayerController playerController)
        {
            // 플레이어의 캐릭터 인스턴스가 이미 존재한다면,
            // 타이틀 -> 인게임 씬 변경이 아니라 던전 내에서의 스테이지 전환을 시도했다는 것임.
            // 따라서, 이러한 경우에는 플레이어 컨트롤러의 위치만 변경하도록 설정.
            if(playerController != null)
            {
                // 이동한 스테이지의 StartPos를 찾기.
                var entryPos = currentStage.transform.Find("StartPos").transform;

                // 플레이어의 위치를 해당 위치로 변경.
                playerController.transform.position = entryPos.position;

                return;
            }

            // TODO: 여기로 넘어오면 타이틀 -> 인게임 씬 변경이라는 뜻.
            //       즉, 유저 데이터에서 보유중인 캐릭터의 정보를 받아와야 함.
        }
    }
}