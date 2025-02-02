using Portfolio.Define;
using UnityEngine;

namespace Portfolio.SD
{
    [CreateAssetMenu(menuName = "Portfolio/StageData")]
    public class SDStage : StaticData
    {
        /// <summary>
        /// 스테이지 이름.
        /// </summary>
        public string stageName;
        /// <summary>
        /// 해당 스테이지에서 이동할 수 있는 스테이지들의 기획 데이터상의 인덱스.
        /// </summary>
        public int[] startPosStageRef;
        /// <summary>
        /// 프리팹 경로.
        /// </summary>
        public string resourcePath;
        /// <summary>
        /// 인스턴스의 스테이지 타입.
        /// </summary>
        public StageType stageType;
    }
}
