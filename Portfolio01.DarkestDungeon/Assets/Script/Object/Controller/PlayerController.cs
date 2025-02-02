using Portfolio.Battle;
using Portfolio.BO;
using Portfolio.Controller;
using Portfolio.DataBase;
using Portfolio.Define;
using Portfolio.UI;
using System.Linq;
using UnityEngine;

namespace Portfolio.Object
{
    /// <summary>
    /// 플레이어 캐릭터의 입력 처리.
    /// 입력을 캐릭터 클래스에서 안하는 이유는?
    ///  - 캐릭터와 플레이어의 입력을 분리함으로써, 캐릭터 클래스를 더 다양하게 사용할 수 있기 때문에.
    ///  - ex) 조종할 수 있는 오브젝트가 더 다양해짐.
    ///  
    /// 플레이어 컨트롤러는 '던전' 지형에서만 사용할 예정임.
    /// 던전에 입장하기 전에 플레이어 컨트롤러에 참전할 캐릭터들을 위치에 맞게 초기화 시키는 기능이 있어야 함.
    /// 캐릭터 하나당 초기화를 하나씩 사용할 예정.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public Collider2D Coll { get; set; }
        protected Rigidbody2D rig;

        public float moveSpeed = 20f;
        private bool canRot;

        private InputController inputController;

        /// <summary>
        /// 전투에 참전할 프레이어 캐릭터의 프로퍼티(최대 4명 까지)
        /// </summary>
        public Character[] playerCharacters = new Character[4];

        /// <summary>
        /// 캐릭터를 고정할 홀더의 트랜스폼.
        /// </summary>
        public Transform[] charHolder = new Transform[4];

        /// <summary>
        /// 캐릭터의 이동 방향.(사실 컨트롤러가 이동하는 거임.)
        /// </summary>
        public Vector2 moveDir;

        /// <summary>
        /// 현재 지정한 캐릭터.
        /// </summary>
        public Actor currentCharacter;
        /// <summary>
        /// 현재 커서를 대고있는 캐릭터.
        /// </summary>
        public Character currentFocusCharacter;


        private void Start()
        {
            // 액터들이 사용하는 컴포넌트들의 참조를 받기.
            Coll = GetComponent<Collider2D>();
            rig = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (inputController == null)
                return;
            InputUpdate();
        }


        /// <summary>
        /// 초기화 시, 플레이어 캐릭터의 참조를 주입받는다.
        /// </summary>
        public void Initialize()
        {
            gameObject.tag = "PlayerController";

            for (int i = 0; i < 4; i++)
            {
                // 암의로 크루세이더의 SD데이터를 가져와 sdCrusader 변수에 넣기.
                var sdCrusader = GameManager.SD.sdCharacters.Where(_ => _.index == 0).SingleOrDefault();

                // 캐릭터 홀더의 자식중 Character를 가진 옵브젝트를 찾아서 tmpChar에 대입.
                var tmpChar = charHolder[i]?.GetComponentInChildren<Character>();

                // tmpChar이 null이라면?
                if (tmpChar == null)
                    // 해당 칸에는 캐릭터 배치를 안했으니 컨티뉴.
                    continue;

                // 플레이어 캐릭터 태그를 Player 태그로 지정.
                // (다른 캐릭터가 존재한다고 가정했을 때, 플레이어의 캐릭터를 구분하기 위함.)
                tmpChar.gameObject.tag = "Player";

                // 플레이어 캐릭터의 레이어를 Character 레이어로 지정.
                tmpChar.gameObject.layer = LayerMask.NameToLayer("Character");

                // 플레이어 캐릭터의 하이라키 상의 부모를 컨트롤러로 지정.
                tmpChar.transform.SetParent(charHolder[i]);

                // 지정된 홀더에 tmpChar로 세팅한 값을 대입.
                playerCharacters[i] = tmpChar;

                // 크루세이더의 SD데이터를 참고해서 해당 캐릭터의 Initialize() 기능 발동.
                playerCharacters[i].Initialize(new BoCharacter(sdCrusader));

                // 0 1 2 3
                // 3 2 1 0
                playerCharacters[i].currentColumn = Mathf.Abs(i - 4);

                // IngameManager의 활성화된 액터 목록에 추가.
                IngameManager.Instance.AddActor(playerCharacters[i]);

            }

        }

        /// <summary>
        /// 함수들을 키에 등록하는 함수.
        /// </summary>
        public void KeyReigstration()
        {
            // 입력 컨트롤러 객체 생성.
            inputController = new InputController();

            // 축 타입 키 등록.
            // 영지, 던전, 전투에서 각각 다른 성능의 키를 가짐.
            // 영지
            //  1. 각종 건축물에 관련된 상호작용.(나중에 자세히 추가하기)
            //  2. 우측 패널에서 캐릭터 선택, 가운데 아래에 있는 슬릇에 데려갈 캐릭터들을 배치.
            // 던전
            //  1. 캐릭터들을 선택해서 '현재 지정한 캐릭터'에 등록하기.
            //   -> 각종 상호작용에 쓰일 스탯을 '현재 지정한 캐릭터'에 등록된 캐릭터에서 가져옴.
            //  2. 인벤토리 드래그로 관리 및 사용.
            //  3. 장비 탈착 기능.
            //  4. 거리에 따라서 왼클릭으로도 상호작용 가능하게 하기.
            // 전투
            //  1. 캐릭터의 속도로 턴을 받는 시스템이기 때문에, 턴을 받은 아군 캐릭터를 '현재 지정한 캐릭터'로 강제 등록됨.
            // 나중에 후술 ㄱㄱ

            // 버튼 타입 키 등록.
            //  1. A, D 키를 이용해서 캐릭터를 좌우로 이동시키기.(사실 컨트롤러를 이동시키는 거임)  
            switch (GameManager.Instance.currentStage)
            {
                case Define.StageType.Wisdom:
                    break;
                case Define.StageType.Dungeon_Selection:
                    break;
                case Define.StageType.Expedition_Readiness:
                    break;
                case Define.StageType.Dungeon:
                    // 던전에서 이동할 때만 해당 키를 사용.
                    inputController.AddAxis("Horizontal", OnMoveHorizontal);
                    inputController.AddButton("Fire1", OnDownMouseLeft_Dungeon, null, null, null);
                    inputController.AddButton("Fire2", OnDownMouseRight_Dungeon,null, null, null);
                    break;
                case Define.StageType.Battle:
                    inputController.AddButton("Fire1", OnDownMouseLeft_Dungeon, null, null, OnNotPressMouse_Battle);
                    inputController.AddButton("Fire2", OnDownMouseRight_Dungeon, null, null, null);
                    break;

            }

            //  2. W 키를 이용해서 'Door' 오브젝트와 상호작용 하기.
            //  3. 스페이스바를 사용해서 'Accesible'오브젝트와 상호작용하기.(마우스로 캐릭터를 지정해서, 해당 캐릭터의 스탯으로 상호작용 함.)
            //   -> 함정도 똑같은 원리로 작동.
            //  4. T 버튼을 눌러서 횃불 점화하기.


            // 테스트용 버튼 TODO : 렌더링 전에 꼭 삭제하기.
            inputController.AddButton("Q", TestButton_ChangeStage0, null, null, null);
            inputController.AddButton("E", TestButton_ChangeStage1, null, null, null);
        }
        /// <summary>
        /// InputController에 등록한 키들을 체크해서, 해당 키에 맞는 함수들을 사용함.
        /// </summary>
        private void InputUpdate()
        {
            // 축 타입의 키 체크.
            foreach (var inputAxis in inputController.inputAxis)
            {
                inputAxis.Value.GetAxisValue(UnityEngine.Input.GetAxisRaw(inputAxis.Key));
            }

            foreach (var inputButton in inputController.inputButton)
            {
                if (UnityEngine.Input.GetButtonDown(inputButton.Key))
                {
                    inputButton.Value.OnDown();
                }
                else if (UnityEngine.Input.GetButton(inputButton.Key))
                {
                    inputButton.Value.OnPress();
                }
                else if (UnityEngine.Input.GetButtonUp(inputButton.Key))
                {
                    inputButton.Value.OnUp();
                }
                else
                {
                    inputButton.Value.OnNotPress();
                }
            }
        }

        #region 입력 구현부.
        private void OnMoveHorizontal(float value)
        {
            moveDir.x = value;
            var newVelocity = moveDir * moveSpeed;
            newVelocity.y = rig.velocity.y;

            rig.velocity = newVelocity;
        }

        private void TestButton_ChangeStage0()
        {
            var stageManager = StageManager.Instance;

            Debug.Log("Q버튼 누름.");
            GameManager.Instance.OnAdditiveLoadingScene(stageManager.ChangeStage(GameManager.User.boAccount.currentStageIndex - 1),
                stageManager.OnChangeStageComplete);
        }
        private void TestButton_ChangeStage1()
        {
            var stageManager = StageManager.Instance;

            Debug.Log("E버튼 누름.");
            GameManager.Instance.OnAdditiveLoadingScene(stageManager.ChangeStage(GameManager.User.boAccount.currentStageIndex + 1),
                stageManager.OnChangeStageComplete);
        }

        /// <summary>
        /// 던전, 플레이어가 마우스 클릭 시 이행될 기능들.
        /// </summary>
        private void OnDownMouseLeft_Dungeon()
        {
            var ray = CameraController.Cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, transform.forward, 1000f);
            Debug.DrawRay(ray, transform.forward * 1000f, Color.red, 2f);

            if (hit)
            {
                Debug.Log($"선택한 오브젝트 : {hit.collider.gameObject.name}");
                // 레이를 쏴서 맞은 오브젝트들의 레이어를 검사하여 특정 오브젝트를 클릭한 경우로 넘어감.
                switch (hit.collider.gameObject.layer)
                {
                    // 아군 캐릭터를 클릭한 경우.
                    case (int)ObjectType.Character:
                        var currentColumn = Mathf.Abs(hit.collider.gameObject.GetComponent<Character>().currentColumn - 4);
                        Debug.Log($"현재 캐릭터의 행 : {currentColumn}");
                        // 현재 지정한 캐릭터를 넘겨주기.
                        currentCharacter = playerCharacters[currentColumn];
                        // 현재 선택한 캐릭터 지정 후, 정보를 교체하는 함수 발동.
                        UIWindowManager.Instance.GetWindow<UIEquipment>().SetAllInformation();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 던전에서 마우스 우클릭 시, 해당 캐릭터(아군만)의 정보를 캐릭터 팝업창에 전달하는 함수.
        /// </summary>
        private void OnDownMouseRight_Dungeon()
        {
            // 게임 씬의 캐릭터에 레이저를 쏴서 감지하는 기능.
            var ray = CameraController.Cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, transform.forward, 1000f);
            Debug.DrawRay(ray, transform.forward * 1000f, Color.red, 2f);

            if (hit)
            {
                Debug.Log($"커서를 대고 있는 오브젝트 : {hit.collider.gameObject.name}");
                // 감지한 오브젝트의 레이어를 감지.
                switch(hit.collider.gameObject.layer)
                {
                    // 레이어의 이름이 '캐릭터'일 경우
                    case (int)ObjectType.Character:
                        // 클릭한 캐릭터의 현재 행을 가져옴.
                        var currentColumn = Mathf.Abs(hit.collider.gameObject.GetComponent<Character>().currentColumn - 4);
                        // 현재 행을 기준으로 현재 커서를 대고있는 캐릭터 변수를 변경
                        currentFocusCharacter = playerCharacters[currentColumn];
                        // 캐릭터 팝업창 열기.
                        UIWindowManager.Instance.GetWindow<UICharacterPopup>().Open();
                        break;
                }
            }
        }

        /// <summary>
        /// 전투중 플레이어가 마우스 커서를 갖다 댈 때 발동할 기능들.
        /// </summary>
        private void OnNotPressMouse_Battle()
        {
            var ray = CameraController.Cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, transform.forward, 1000f);
            Debug.DrawRay(ray, transform.forward * 1000f, Color.blue, 2f);

            if (hit)
            {
                Debug.Log($"선택한 오브젝트 : {hit.collider.gameObject.name}");
                switch (hit.collider.gameObject.layer)
                {
                    // 마우스 커서가 Enemy를 향하고 있으면 발동할 함수.
                    case (int)ObjectType.Enemy:
                        // 현재 전투중인 EnemyTile의 간택당한 타겟에 hit정보를 넘기기.
                        BattleManager.Instance.currentEnemyTile.currentPointTarget = hit.collider.gameObject.GetComponent<Enemy>();
                        // 해당 몬스터의 정보를 띄우는 UI 오픈.
                        UIWindowManager.Instance.GetWindow<UIMonsterInfo>().Open();
                        break;
                    // 따로 지정하지 않은 것들을 향할 경우.
                    default:
                        // 몬스턴Info UI 닫기.
                        UIWindowManager.Instance.GetWindow<UIMonsterInfo>().Close();
                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// 플레이어 컨트롤러의 이동을 강제로 멈출 때 사용할 기능.
        /// </summary>
        public void StopPlayer()
        {
            rig.velocity = Vector3.zero;
        }

        public void PlayerDataSave()
        {
            // 플레이어 컨트롤러의 마지막 위치.
            var lastPos = this.transform.position;
            // 더미서버에 존재하는 유저 DB에 플레이어 컨트롤러의 마지막 위치를 저장.
            DummyServer.Instance.userData.lastPos = lastPos;
            // 플레이어가 마지막으로 위치했던 스테이지를 변경.
            DummyServer.Instance.userData.lastStageIndex = GameManager.User.boAccount.currentStageIndex;

            // 각종 재화들도 저장하기.
            DummyServer.Instance.userData.bust = GameManager.User.boAccount.bust;
            DummyServer.Instance.userData.portrait = GameManager.User.boAccount.portrait;
            DummyServer.Instance.userData.bond = GameManager.User.boAccount.bond;
            DummyServer.Instance.userData.emblem = GameManager.User.boAccount.emblem;
            DummyServer.Instance.userData.fragment = GameManager.User.boAccount.fragment;
            DummyServer.Instance.userData.gold = GameManager.User.boAccount.gold;
            // 더미 서버 저장하기.
            DummyServer.Instance.Save();
        }

        /// <summary>
        /// 어플리케이션 종료 시, 플레이어 컨트롤러의 현재 위치와, 현재 스테이지 index를 유저 DB에 저장하기.
        /// </summary>
        private void OnApplicationQuit()
        {
            PlayerDataSave();
        }
    }
}