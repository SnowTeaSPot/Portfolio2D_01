using Portfolio.Battle;
using Portfolio.Object;
using TMPro;

namespace Portfolio.UI
{
    /// <summary>
    /// 전투중, 적 유닛에게 커서를 대면 해당 유닛의 정보를 표시할 UI
    /// </summary>
    public class UIMonsterInfo : UIWindow
    {
        /// <summary>
        /// 현재 전투중인 EnemyTile
        /// </summary>
        public EnemyTile currentEnemyTile;

        /// <summary>
        /// 대충 몬스터 정보
        /// </summary>
        public TextMeshProUGUI nickName;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI tribe;
        public TextMeshProUGUI defName;
        public TextMeshProUGUI def;
        public TextMeshProUGUI evade;
        public TextMeshProUGUI speed;
        public TextMeshProUGUI stunResist;
        public TextMeshProUGUI addictedResist;
        public TextMeshProUGUI bleedingResist;
        public TextMeshProUGUI weakeningResist;
        public TextMeshProUGUI movingResist;

        public override void Start()
        {
            base.Start();
        }

        public override void Open(bool force = false)
        {
            base.Open(force);

            // UI가 열릴 때, 모든 정보를 새로 세팅하는 기능.
            SetAllInformation();
        }

        /// <summary>
        /// 매개변수로 받은 문자열을 세팅된 TMP로 변환하는 기능.
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="information"></param>
        public void SetInformation(TextMeshProUGUI tmp, string information)
        {
            tmp.text = $"{information}";
        }

        public void SetAllInformation()
        {
            // 배틀 매니저에서 현재 전투중인 EnemyTile을 가져오기.
            currentEnemyTile = BattleManager.Instance.currentEnemyTile;
            var currentEnemy = currentEnemyTile.currentPointTarget.GetComponent<Enemy>();

            // 각종 정보 갱신.
            SetInformation(nickName, currentEnemy.boEnemy.nickName);
            SetInformation(hp, $"{currentEnemy.boEnemy.currentHp} / {currentEnemy.boEnemy.maxHp}");
            SetInformation(tribe, $"{currentEnemy.tribe}");
            if (currentEnemy.boEnemy.currentDef > 0)
            {
                defName.gameObject.SetActive(true);
                SetInformation(def, $"{currentEnemy.boEnemy.currentDef}%");
            }
            else
                defName.gameObject.SetActive(false);
            SetInformation(evade, $"{currentEnemy.boEnemy.currentEvade}");
            SetInformation(speed, $"{currentEnemy.boEnemy.currentSpeed}");
            SetInformation(stunResist, $"{currentEnemy.boEnemy.currentStunResist}%");
            SetInformation(addictedResist, $"{currentEnemy.boEnemy.currentAddictedResist}%");
            SetInformation(bleedingResist, $"{currentEnemy.boEnemy.currentBleedingResist}%");
            SetInformation(weakeningResist, $"{currentEnemy.boEnemy.currentWeakeningResist}%");
            SetInformation(movingResist, $"{currentEnemy.boEnemy.currentMovingResist}%");
        }
    }
}