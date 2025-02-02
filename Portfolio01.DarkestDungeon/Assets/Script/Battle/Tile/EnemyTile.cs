using Portfolio.BO;
using Portfolio.Controller;
using Portfolio.Object;
using Portfolio.Resource;
using Portfolio.SD;
using Portfolio.Util;
using System.Linq;
using UnityEngine;

namespace Portfolio.Battle
{
    /// <summary>
    /// 전투 타일
    /// </summary>
    public class EnemyTile : Tile
    {
        /// <summary>
        /// 전투에 참전한 적 유닛의 프로퍼티( 최대 4명 )
        /// </summary>
        public Actor[] enemyCharacter = new Actor[4];
        /// <summary>
        /// 적 유닛을 고정할 홀더.
        /// </summary>
        public Transform[] enemyHolder = new Transform[4];
        /// <summary>
        /// 현재 지정 타겟.
        /// </summary>
        public Actor currentPointTarget;

        /// <summary>
        /// 현재 EnemyTile의 SD데이터
        /// </summary>
        public SDEnemyTile currentTile;

        /// <summary>
        /// 시점 변경을 위해 카메라 컨트롤러를 가져옴.
        /// </summary>
        public CameraController cameraController;

        private void Start()
        {
            // 원래 EnemySetting 함수는 던전 맵에 입장 후, 로그라이크 알고리즘에 따라 맵을 생성할 때 발동해야 함.
            // 근데 아직 그런 로직이 없으니 맵에 나와있는 EnemyTile의 Start 콜백 함수를 이용합시다.
            TileSetting(0, 1);
            EnemySetting();
        }

        public override void Initialize()
        {
            base.Initialize();
            // 플레이어 컨트롤러와 똑같은 순서로 진행됨.
            // 1. EnemyHolder에 유닛을 배치함.
            // 2. 배치가 끝나면 Initialize()함수를 발동해서 해당 유닛을 BattleManager에 저장함.
            // 3. 그 후에 입맛대로 사용하기.

            for (int i = 0; i < currentTile.enemyArray.Length; i++)
            {
                if (enemyHolder[i].transform.GetChild(0) == null)
                    return;
                // 고정용 홀더에 배치된 유닛에게 Actor 컴포넌트를 찾아서 tmpEnemy에 대입함.
                // Todo : 이건 나중에 수정해야 함;;
                // 플레이어블 캐릭터가 적 유닛으로 등장하면 Character클래스를, 적 유닛이 오면 Enemy를 줘야 하기 때문임.
                var tmpEnemy = enemyHolder[i].transform.GetChild(0).GetComponent<Actor>();

                // tmpEnemy가 null이라면?
                if (tmpEnemy == null)
                    // 해당 칸에는 유닛 배치를 안했으니 컨티뉴.
                    continue;

                // 해당 유닛의 태그를 Enemy 태그로 지정.
                tmpEnemy.gameObject.tag = "Enemy";

                // 해당 유닛의 레이어를 Enemy 레이어로 지정.
                tmpEnemy.gameObject.layer = LayerMask.NameToLayer("Enemy");

                // 저장된 홀더에 tmpEnemy에 세팅한 값을 대입.
                enemyCharacter[i] = tmpEnemy;

                Debug.Log($"{i}번 간에 배치된 적 유닛 : {tmpEnemy.name}");
            }

            // 타일 초기화 시, 카메라 컨트롤러를 찾아서 넣어주기.
            cameraController = FindObjectOfType<CameraController>();            
        }

        /// <summary>
        /// EnemyTile에 적 유닛 조합을 등록하는 기능.
        /// </summary>
        /// <param name="minIndex">최소 인덱스</param>
        /// <param name="maxIndex">최대 인덱스</param>
        public void TileSetting(int minIndex, int maxIndex)
        {
            var sd = GameManager.SD;
            var resourceManager = ResourceManager.Instance;

            // 매개변수의 Index 사이의 값을 랜덤하게 굴려서 변수에 대입.
            int randIndex = Random.Range(minIndex, maxIndex + 1);
            // 적 유닛의 Index는 10만번대에서 시작이기 때문에, 10만을 더하기.
            randIndex += 100000;

            // 랜덤한 index의 조합을 SDEnemyTile에 가져와 대입하기.
            currentTile = sd.sdEnemyTile.Where(_=> _.index == randIndex).SingleOrDefault();

            // enmeyArray에 등록된 index의 갯수만큼 반복.
            for(int i = 0;i < currentTile.enemyArray.Length;i++) 
            {
                // sdEnemyTile.enemyArray의 배열 순서가 몬스터의 초기 위치 순서임.
                // 해당 순서대로 기획 데이터를 하나씩 불러와서 sdEnemy에 넣기.
                var sdEnemy = sd.sdEnemy.Where(_=>_.index == currentTile.enemyArray[i]).SingleOrDefault();

                if (sdEnemy != null)
                    // 불러온 기획 데이터의 경로를 통해 몬스터 풀에 등록.
                    resourceManager.LoadPoolableObject<Enemy>(sdEnemy.resourcePath, 1);
                else
                    continue;
            }
        }

        /// <summary>
        /// EnemyHolder에 몬스터를 접어 넣는 기능.
        /// </summary>
        private void EnemySetting()
        {
            // Enemy클래스를 가진 풀을 가져오기.
            var enemyPool = PoolManager.Instance.GetPool<Enemy>();
            var sd = GameManager.SD;
            
            // 현재 EnemyTile의 몬스터 배열 수만큼 반복.
            for (int i = 0; i < currentTile.enemyArray.Length; i++)
            {
                // i번째의 몬스터 index를 통해 해당 몬스터의 SD데이터를 변수에 담기.
                var sdEnemy = sd.sdEnemy.Where(_ => _.index == currentTile.enemyArray[i]).SingleOrDefault();

                // 이제 몬스터 풀에서 몬스터 객체를 하나 가져온 후, 데이터를 채워서 사용하기만 하면 됨.
                //  -> 근데 우리가 사용중인 몬스터 풀에는 여러 종류의 몬스터가 존재할 예정임.
                //     그럼 몬스터 풀에서 몬스터를 가져올 때, 내가 생성할 몬스터 객체와 동일한 종류의 객체를 찾아야 함.
                var enemyName = sdEnemy.resourcePath.Remove(0, sdEnemy.resourcePath.LastIndexOf('/') + 1);
                
                // 현재 몬스터의 이름으로 동일한 객체를 풀에서 찾기.
                // 그러나 반복문에서 새로운 문자열을 생성하는 방식은 존나 좆같으니깐 나중에 바꿔봅시다.
                var enemy = enemyPool.GetObject(_ => _.name == enemyName);

                // 몬스터를 가져오지 못했다면 컨티뉴.
                if(enemy == null)
                    continue;

                //해당 유닛의 하이라키 상의 부모를 타일로 지정.
                enemy.transform.SetParent(enemyHolder[i]);
                // 해당 유닛의 포지션을 부모의 0, 0, 0으로 변경.
                enemy.transform.localPosition = Vector3.zero;
                // 해당 유닛의 초기화 작업.
                enemy.Initialize(new BoEnemy(sdEnemy));
            }

            // 세팅이 끝았으면 Initialize() 기능을 통해 홀더에 배치된 유닛의 기본값을 설정해주기.
            Initialize();
        }

        /// <summary>
        /// EnemyTile에 Player가 도달했을 때 실행시킬 기능
        /// </summary>
        /// <param name="collision">플레이어 감지용 콜라이더.</param>
        public override void OnEnterPlayer(Collider2D collision)
        {
            // 겹친 콜라이더의 태그가 "PlayerController"가 아니라면 
            if (!collision.CompareTag("PlayerController"))
                // 리턴
                return;

            var ingameManager = IngameManager.Instance;

            // 카메라 위치를 EnemyTile로 고정.
            cameraController.SetTarget(transform);
            
            //비활성화 된 몬스터를 찾기.
            for(int i = 0; i < currentTile.enemyArray.Length; i++)
            {
                // 해당 몬스터를 활성화 상태로 변경.
                enemyCharacter[i].gameObject.SetActive(true);
                // 인게임 매니저의 활성화 액터 목록에 등록하기.
                ingameManager.AddActor(enemyCharacter[i]);
                enemyCharacter[i].currentColumn = i;
            }

            var playerController = FindObjectOfType<PlayerController>();

            // 게임 매니저의 currentStage를 Battle로 변경함.
            GameManager.Instance.currentStage = Define.StageType.Battle;
            // 플레이어 컨트롤러의 위치를 해당 타일과 똑같이 변경.
            playerController.transform.position = transform.position;
            // 전투가 시작됐으니 키세팅을 다시 재배치.(전투가 끝나고 던전으로 돌아갈때도 해야 함.)
            playerController.KeyReigstration();
            // 전투 진행을 위해 플레이어 컨트롤러의 이동을 강제로 멈춤.
            playerController.StopPlayer();

            // 모든 작업이 끝났으니 BattleManager의 currentEnemyTile에 전달하기.
            BattleManager.Instance.currentEnemyTile = this;
        }
    }
}