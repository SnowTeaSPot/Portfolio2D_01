using Portfolio.Controller;
using Portfolio.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio.Battle
{
    /// <summary>
    /// 모든 타일의 부모가 될 Tile 클래스.
    /// 타일이 가져야 할 기초적인 기능만 담겨있음.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        /// <summary>
        /// 현재 타일이 통로인지 방인지 구분하는 변수.
        /// </summary>
        public TileType tileType;

        /// <summary>
        /// EnemyTile에 들어온 플레이어를 감지하기 위한 CollTrigger 클래스.
        /// </summary>
        public CollTriggerController tileColl;

        /// <summary>
        /// 맵 생성 시, 타일의 기본값 세팅.
        /// </summary>
        public virtual void Initialize()
        {
            tileColl.Initialize(OnEnterPlayer, null, null);
        }

        /// <summary>
        /// 플레이어가 타일에 들어올 경우에 발동할 기능.
        /// </summary>
        /// <param name="collision"></param>
        public virtual void OnEnterPlayer(Collider2D collision)
        {

        }
    }
}
