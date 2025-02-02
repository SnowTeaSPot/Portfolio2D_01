using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Portfolio
{
    /// <summary>
    /// 타이틀 UI
    /// </summary>
    public class UITitle : MonoBehaviour
    {
        /// <summary>
        /// 현재 진행 상태를 나타내는 TMP.
        /// </summary>
        public TextMeshProUGUI loadState;

        /// <summary>
        /// 진행 상태를 나타내는 하단 바.
        /// </summary>
        public Image loadGauge;

        /// <summary>
        /// 로딩 상태 텍스트 설정.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(string state)
        {
            loadState.text = $"Load {state}...";
        }

        /// <summary>
        /// 로딩 바 애니메이션 처리
        /// </summary>
        /// <param name="loadPer">현재 로드 퍼센테이지</param>
        /// <returns></returns>
        public IEnumerator LoadGaugeUpdate( float loadPer)
        {
            // ui의 fillAmount 값이랑 파라미터로 전달받은 퍼센트 값이 근사하지 않다면, 반복하기.
            while (!Mathf.Approximately(loadGauge.fillAmount, loadPer))
            {
                /// Mathf.Lerp(float a, float b, float c) -> a가 b가 될 때까지 c만큼의 쿨타임을 가짐.
                loadGauge.fillAmount = Mathf.Lerp(loadGauge.fillAmount, loadPer, Time.deltaTime * 2f);

                yield return null;
            }
        }
    }
}

