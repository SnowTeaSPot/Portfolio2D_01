using Portfolio.Object;
using TMPro;

namespace Portfolio.UI
{
    /// <summary>
    /// 던전 탐색 중, PlayerController의 currentCharacter에 담긴 캐릭터의 현재 스탯, 장비, 장신구를 나타내는 UI
    /// </summary>
    public class UIEquipment : UIWindow
    {
        /// <summary>
        /// 현재 지정한 캐릭터를 가져올 playerController
        /// </summary>
        public PlayerController playerController;

        /// <summary>
        /// 타겟의 HP
        /// </summary>
        public TextMeshProUGUI targetHP;
        /// <summary>
        /// 타겟의 스트레스 수치.
        /// </summary>
        public TextMeshProUGUI targetStress;
        /// <summary>
        /// 타겟의 현재 명중 보정치.
        /// </summary>
        public TextMeshProUGUI targetAccuracyCorrect;
        /// <summary>
        /// 타겟의 현재 치명타.
        /// </summary>
        public TextMeshProUGUI targetCritical;
        /// <summary>
        /// 타겟의 현재 공격력.
        /// </summary>
        public TextMeshProUGUI targetATK;
        /// <summary>
        /// 타겟의 회피치.
        /// </summary>
        public TextMeshProUGUI targetEvade;
        /// <summary>
        /// 타겟의 방어력.
        /// </summary>
        public TextMeshProUGUI targetDef;
        /// <summary>
        /// 타겟의 현재 속도.
        /// </summary>
        public TextMeshProUGUI targetSpeed;

        /// <summary>
        /// 매개변수로 받은 문자열을 세팅된 Tmp로 변환하는 기능.
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="information"></param>
        public void SetInformation(TextMeshProUGUI tmp, string information)
        {
            // 바꿔줌.
            tmp.text = $"{information}";
        }

        /// <summary>
        /// SetInformation() 함수를 사용해 모든 정보를 한꺼번에 바꾸는 기능.
        /// </summary>
        public void SetAllInformation()
        {
            // 하이라키에서 플레이어 컨트롤러를 찾아서 대입하기.
            playerController = FindObjectOfType<PlayerController>();

            // 현재 지정한 캐릭터가 없다면 리턴.
            if (playerController.currentCharacter == null)
                return;

            var currentCharacter = playerController.currentCharacter.GetComponent<Character>().boCharacter;

            // 각종 정보 변환하기.
            SetInformation(targetHP, $"{currentCharacter.currentHp} / {currentCharacter.maxHp}");
            SetInformation(targetStress, $"{currentCharacter.currentStress} / {200}");
            SetInformation(targetAccuracyCorrect, currentCharacter.curAccuracyCorrect.ToString());
            SetInformation(targetCritical, $"{currentCharacter.currentCritical}%");
            SetInformation(targetATK, $"{currentCharacter.currentMinATK}-{currentCharacter.currentMaxATK}");
            SetInformation(targetEvade, $"{currentCharacter.currentEvade}");
            SetInformation(targetDef, $"{currentCharacter.currentDef}%");
            SetInformation(targetSpeed, $"{currentCharacter.currentSpeed}");
        }
    }
}