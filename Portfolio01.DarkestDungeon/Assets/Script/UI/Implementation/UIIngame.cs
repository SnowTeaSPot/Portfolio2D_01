using Portfolio.Object;
using Portfolio.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.UI
{
    /// <summary>
    /// 인 게임 내 잡다한 UI 요소들을 관리할 클래스 (체력 바, 스트레스 바, 기타 hud UI 등등)
    /// </summary>
    public class UIIngame : UIWindow
    {
        /// <summary>
        /// 인게임 ui요소 중 플레이어 데이터를 기반으로 가공한 ui가 많기 때문에
        /// 참조를 미리 받아둔 상태에서 사용함.
        /// </summary>
        public PlayerController playerController;
        /// <summary>
        /// 모든 월드 UI 요소를 갖는 UI 캔버스.
        /// </summary>
        public Transform worldUICanvas;
        /// <summary>
        /// 하이라키 상의 모든 HpBar를 가질 리스트.
        /// </summary>
        private List<HpBar> allHpBar = new List<HpBar>();
        /// <summary>
        /// 하이라키 상의 모든 StressBar를 가질 리스트.
        /// </summary>
        private List<StressBar> allStressBar = new List<StressBar>();

        private void Update()
        {
            HpBarUpdate();
            StressBarUpdate();
        }

        /// <summary>
        /// 전체 체력 바를 업데이트 하는 기능.
        /// </summary>
        private void HpBarUpdate()
        {
            for(int i = 0; i < allHpBar.Count; i++)
            {
                allHpBar[i]?.HpBarUpdate();
            }
        }

        /// <summary>
        /// 전체 스트레스 바를 업데이트 하는 기능.
        /// </summary>
        private void StressBarUpdate()
        {
            for(int i = 0; i < allStressBar.Count; i++)
            {
                allStressBar[i]?.StressBarUpdate();
            }
        }

        /// <summary>
        /// 매개변수로 받은 액터의 정보를 기준으로 체력바를 생성하여
        /// 전체 체력바 리스트에 추가하는 기능.
        /// </summary>
        /// <param name="target"></param>
        public void AddHpBar(Actor target)
        {
            var hpBar = PoolManager.Instance.GetPool<HpBar>().GetObject();

            // 풀에서 가져온 hp바의 부모를 월드ui캔버스로 설정.
            hpBar.transform.SetParent(worldUICanvas);

            // 타겟의 데이터를 기반으로 hp 바 초기화 함수 선언.
            hpBar.Initialize(target);
            hpBar.gameObject.SetActive(true);

            allHpBar.Add(hpBar);
        }

        /// <summary>
        /// 위 함수와 동일함.
        /// </summary>
        /// <param name="target"></param>
        public void AddStressBar(Actor target)
        {
            var StressBar = PoolManager.Instance.GetPool<StressBar>().GetObject();

            // 풀에서 가져온 Stress바의 부모를 월드ui캔버스로 설정.
            StressBar.transform.SetParent(worldUICanvas);

            // 타겟의 데이터를 기반으로 Stress 바 초기화 함수 선언.
            StressBar.Initialize(target);
            StressBar.gameObject.SetActive(true);

            allStressBar.Add(StressBar);
        }

        /// <summary>
        /// 스테이지 전환 시, 현재 스테이지에서 사용하던 UI 요소들을 비우는 작업.
        /// </summary>
        public void Clear()
        {
            var hpBarPool = PoolManager.Instance.GetPool<HpBar>();
            var StressBarPool = PoolManager.Instance.GetPool<StressBar>();

            for(int i = 0; i < allHpBar.Count; i++)
            {
                hpBarPool.Return(allHpBar[i]);
            }
            for(int i = 0; i < allStressBar.Count; i++)
            {
                StressBarPool.Return(allStressBar[i]);
            }

            allHpBar.Clear();

            allStressBar.Clear();
        }
    }
}