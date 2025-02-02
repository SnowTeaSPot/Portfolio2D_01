using Portfolio.Object;
using Portfolio.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.UI
{
    /// <summary>
    /// Character가 사용할 스트레스 게이지 UI
    /// </summary>
    public class StressBar : MonoBehaviour, IPoolable
    {
        /// <summary>
        /// stress를 출력하고자 하는 타겟 액터 참조.
        /// </summary>
        public Actor target;
        /// <summary>
        /// stress 게이지 이미지 참조.
        /// </summary>
        public Image gauge;
        /// <summary>
        /// 풀링 오브젝트의 인터페이스 구현.
        /// </summary>
        public bool CanRecycle { get; set; } = true;

        /// <summary>
        /// 초기화 시, stress 출력하고자 하는 타겟의 참조를 넘겨받음.
        /// </summary>
        /// <param name="target">stress 게이지를 출력하고자 하는 타겟.</param>
        public void Initialize(Actor target)
        {
            this.target = target;

            // 툴 매니저에서 사용하는 오브젝트들은 별도의 전용 폴더에 보관하고 있음.
            // 근데 풀에서 꺼내 사용할 때는 폴더에서 월드 캔버스로 하이러키상의 부모가 변경이 됨.
            // 이 때, Transform -> RectTransform으로 스케일링이 발생하므로 부모를 1, 1, 1의 크기로 다시 스케일링함.
            transform.localScale = Vector3.one;
        }


        public void StressBarUpdate()
        {
            if (target == null)
                return;

            if (target.State == Define.State.Dead)
            {
                PoolManager.Instance.GetPool<StressBar>().Return(this);
                return;
            }

            transform.position = target.transform.position + Vector3.down * 2.75f;
        }
    }
}