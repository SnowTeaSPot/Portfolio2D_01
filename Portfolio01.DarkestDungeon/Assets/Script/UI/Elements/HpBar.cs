using Portfolio.Object;
using Portfolio.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.UI
{
    /// <summary>
    /// Character와 Enemy가 사용할 체력 게이지 UI
    /// </summary>
    public class HpBar : MonoBehaviour , IPoolable
    {
        /// <summary>
        /// hp를 출력하고자 하는 타겟 액터 참조.
        /// </summary>
        public Actor target;
        /// <summary>
        /// hp 게이지 이미지 참조.
        /// </summary>
        public Image gauge;
        /// <summary>
        /// 풀링 오브젝트의 인터페이스 구현.
        /// </summary>
        public bool CanRecycle { get; set; } = true;
        
        /// <summary>
        /// 초기화 시, hp를 출력하고자 하는 타겟의 참조를 넘겨받음.
        /// </summary>
        /// <param name="target">hp 게이지를 출력하고자 하는 타겟.</param>
        public void Initialize(Actor target)
        {
            this.target = target;

            // 툴 매니저에서 사용하는 오브젝트들은 별도의 전용 폴더에 보관하고 있음.
            // 근데 풀에서 꺼내 사용할 때는 폴더에서 월드 캔버스로 하이러키상의 부모가 변경이 됨.
            // 이 때, Transform -> RectTransform으로 스케일링이 발생하므로 부모를 1, 1, 1의 크기로 다시 스케일링함.
            transform.localScale = Vector3.one;
        }
        
        /// <summary>
        /// 타겟의 hp가 변경될 때 호출할 함수.
        /// </summary>
        public void HpBarUpdate()
        {
            // 타겟이 존재하는지 확인.
            if (target == null)
                return;

            // 타겟이 죽었다면?
            if (target.State == Define.State.Dead)
            {
                // 체력바를 다시 풀에 반환.
                PoolManager.Instance.GetPool<HpBar>().Return(this);
                return;
            }

            // 위치 조정 및 게이지 업데이트 구문.
            transform.position = target.transform.position + Vector3.down * 2.5f;
            gauge.fillAmount = target.boActor.currentHp / target.boActor.maxHp;
        }
    }
}
