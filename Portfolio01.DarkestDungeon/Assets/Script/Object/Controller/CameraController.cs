using Portfolio.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Object
{
    /// <summary>
    /// 던전 탐색 시, 플레이어를 따라다닐 카메라를 다루는 컨트롤러.
    /// 
    ///  카메라는 던전 탐색 이외엔 항상 고정이기 때문에 아래의 함수는 던전 탐색만을 상정하고 작성하기.
    ///  애초에 0, 0, 0을 기준으로 오브젝트를 설치하고, 플레이어 컨트롤러를 0, 0, 0으로 두면 됨.
    ///  
    ///  그럼 플레이어 컨트롤러를 초기화 할 때마다 위치를 0, 0, 0으로 바꿀 필요가 있음.(이건 플레이어 컨트롤러 에서 하면 됨.)
    ///  
    ///  카메라 컨트롤러의 초기화는 StageChange() 함수에서 진행하기.
    ///   -> 스테이지가 바뀌면 보통 맵 자체가 바뀌기 때문임.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// 플레이어 컨트롤러를 참조할 변수.
        /// </summary>
        public PlayerController player = null;

        /// <summary>
        /// 방이나 통로는 카메라의 최소 최대치가 다름.
        /// 어차피 방이나 통로의 길이 자체는 똑같기 때문에 해당 오브젝트를 만들 때, 해당 오브젝트의 카메라 범위를 나타내는 인스턴스를 만들어두기.
        /// </summary>
        [SerializeField] public float minX = 0;
        [SerializeField] public float maxX = 0;
        [SerializeField] public float minY = 0;
        [SerializeField] public float maxY = 0;

        /// <summary>
        /// 타겟의 포지션을 계속 카운팅 하기 위해 멤버 변수로 설정함.
        /// </summary>
        private Transform targetPos;

        /// <summary>
        /// 스테이지 컨트롤러에서 해당 스테이지의 카메라 범위를 가져와야 하기 때문.
        /// </summary>
        public StageManager stageManager;

        public static Camera Cam { get; set; }

        public float cameraSpeed = 5;

        /// <summary>
        /// 카메라 컨트롤러의 초기화.
        /// </summary>
        public void Initialize()
        {
            player = FindObjectOfType<PlayerController>();
            Cam = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            // 플레이어 컨트롤러를 참조하고 있을 경우.
            if (player == null)
                return;

            // 지정한 타겟이 없다면?
            if (targetPos == null)
                // 플레이어 컨트롤러를 따라가는 함수 실행.
                PlayerFollow();
            // 지정한 타겟이 있다면.
            else
                SetPosition(true);
        }

        private void Update()
        {
            // 여기다 Math.Clamp 함수를 추가 해야 함.
        }

        /// <summary>
        /// 카메라가 플레이어 컨트롤러를 따라가게 하는 함수.
        /// </summary>
        private void PlayerFollow()
        {
            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;

            Vector3 target = new Vector3(playerX, playerY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, target, cameraSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 카메라가 스테이지를 벗어나지 못하게 제한을 두는 함수.
        /// </summary>
        private void CameraClamp()
        {

        }

        /// <summary>
        /// 각종 상호작용이나 전투 시, 카메라가 확대되는 경우가 있음.
        /// 그때 사용할 함수.
        /// </summary>
        public void CameraZoom()
        {

        }

        /// <summary>
        /// 카메라가 특정한 타겟을 추적할 때 사용하는 기능.
        /// 스테이지, 전투 시 카메라 고정 등에 사용함.
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            targetPos = target;

            // 추적한 타겟이 PlayerController일 경우
            if (target.CompareTag("PlayerController"))
                // PlayerFollow로 넘어가기 위해 targetPos를 Null로 변경함.
                targetPos = null;

            transform.position = targetPos.position;
        }

        private void SetPosition(bool isLerp)
        {
            if (isLerp)
            {
                float targetX = targetPos.transform.position.x;
                float targetY = targetPos.transform.position.y;

                Vector3 target = new Vector3(targetX, targetY, -10f);

                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * 3f);
            }
            else
            {
                float targetX = targetPos.transform.position.x;
                float targetY = targetPos.transform.position.y;

                Vector3 target = new Vector3(targetX, targetY, -10f);

                transform.position = target;
            }
        }
    }
}