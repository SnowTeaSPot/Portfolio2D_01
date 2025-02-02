using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Portfolio.Define.UI;

namespace Portfolio.UI
{
    /// <summary>
    /// 던전 탐색 시, 사용할 UI
    /// </summary>
    public class UIDungeon : UIWindow
    {
        /// <summary>
        /// UIInventory의 위치를 가질 변수.
        /// </summary>
        public Transform uiInventory;
        /// <summary>
        /// UIMap의 위치를 가질 변수
        /// </summary>
        public Transform uiMap;

        // Map과 Inventory를 바꾸는 버튼
        public Button buttonMap;    // 맵 버튼
        public Button buttonInventory;  // 인벤토리 버튼

        public override void Start()
        {
            base.Start();

            // UI위치 참조.
            uiInventory = transform.Find("Panel/RightSide/UIInventory");
            uiMap = transform.Find("Panel/RightSide/UIMap");

            // 버튼 대입.
            buttonMap.onClick.AddListener(() => OnSideChange(DungeonRightSide.Map));
            buttonInventory.onClick.AddListener(() => OnSideChange(DungeonRightSide.Inventory));
        }

        /// <summary>
        /// RightSide의 상태를 변경하는 함수.
        /// </summary>
        /// <param name="side"></param>
        private void OnSideChange(DungeonRightSide side)
        {
            switch (side)
            {
                case DungeonRightSide.Map:
                    // UIInventory를 닫기.
                    UIWindowManager.Instance.GetWindow<UIInventory>().Close();
                    // UIMap을 열기.
                    UIWindowManager.Instance.GetWindow<UIMap>().Open();
                    break;
                case DungeonRightSide.Inventory:
                    // UIMap을 닫기.
                    UIWindowManager.Instance.GetWindow<UIMap>().Close();
                    // UIInventory를 닫기.
                    UIWindowManager.Instance.GetWindow<UIInventory>().Open();
                    break;
            }
        }
    }
}