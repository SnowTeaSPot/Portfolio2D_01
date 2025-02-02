using Portfolio.Object;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.UI
{
    /// <summary>
    /// 캐릭터 상세 내용을 전달하는 팝업창 UI
    /// </summary>
    public class UICharacterPopup : UIWindow
    {
        /// <summary>
        /// 캐릭터 이름.
        /// </summary>
        public TextMeshProUGUI nickName;
        /// <summary>
        /// 캐릭터 직업
        /// </summary>
        public TextMeshProUGUI job;
        /// <summary>
        /// 현재 스킨 번호
        /// </summary>
        public TextMeshProUGUI recolorCount;
        /// <summary>
        /// 기절 저항력 수치
        /// </summary>
        public TextMeshProUGUI stunCount;
        /// <summary>
        /// 중독 저항력 수치
        /// </summary>
        public TextMeshProUGUI addictedCount;
        /// <summary>
        /// 질병 저항력 수치
        /// </summary>
        public TextMeshProUGUI diseaseCount;
        /// <summary>
        /// 죽음의 일격 저항
        /// </summary>
        public TextMeshProUGUI deathDoorCount;
        /// <summary>
        /// 이동 저항력 수치
        /// </summary>
        public TextMeshProUGUI movingCount;
        /// <summary>
        /// 출혈 저항력 수치
        /// </summary>
        public TextMeshProUGUI bleedingCount;
        /// <summary>
        /// 약화 저항력 수치
        /// </summary>
        public TextMeshProUGUI weakeningCount;
        /// <summary>
        /// 함정 해제 성공률
        /// </summary>
        public TextMeshProUGUI trapDisarmCount;

        #region 기본 능력치
        /// <summary>
        /// 최대 체력
        /// </summary>
        public TextMeshProUGUI maxHp;
        /// <summary>
        /// 회피치
        /// </summary>
        public TextMeshProUGUI evade;
        /// <summary>
        /// 방어력
        /// </summary>
        public TextMeshProUGUI def;
        /// <summary>
        /// 속도
        /// </summary>
        public TextMeshProUGUI speed;
        /// <summary>
        /// 명중 보정
        /// </summary>
        public TextMeshProUGUI accuracy;
        /// <summary>
        /// 치명타
        /// </summary>
        public TextMeshProUGUI crit;
        /// <summary>
        /// 공격력 수치
        /// </summary>
        public TextMeshProUGUI damage;
        #endregion

        /// <summary>
        /// 긍정적 기벽 리스트(최대 5개)
        /// </summary>
        public List<TextMeshProUGUI> positiveEccenList = new List<TextMeshProUGUI>();
        /// <summary>
        /// 부정적 기벽 리스트(최대 5개)
        /// </summary>
        public List<TextMeshProUGUI> negativeEccenList = new List<TextMeshProUGUI>();

        /// <summary>
        /// 전투 기술 아이콘 리스트.
        /// </summary>
        public List<Image> battleSkillIcon = new List<Image>();
        /// <summary>
        /// 캠프 기술 아이콘 리스트.
        /// </summary>
        public List<Image> campingSkillIcon = new List<Image>();

        /// <summary>
        /// 무기 이미지.
        /// </summary>
        public Image weaponImage;
        /// <summary>
        /// 방어구 이미지.
        /// </summary>
        public Image armorImage;
        /// <summary>
        /// 무기 레벨 텍스트
        /// </summary>
        public TextMeshProUGUI weaponLevel;
        /// <summary>
        /// 방어구 레벨 텍스트
        /// </summary>
        public TextMeshProUGUI armorLevel;
        /// <summary>
        /// 장신구 슬롯 1
        /// </summary>
        public Image accSlot0;
        /// <summary>
        /// 장신구 슬롯 2
        /// </summary>
        public Image accSlot1;

        /// <summary>
        /// 현재 커서를 대고있는 캐릭터를 가져올 플레이어 컨트롤러 변수.
        /// </summary>
        public PlayerController playerController;

        public override void Open(bool force = false)
        {
            base.Open(force);

            SetAllInformation();
        }

        /// <summary>
        /// 매개변수로 받은 문자열을 TMP로 변환하는 기능.
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="information"></param>
        public void SetInformation(TextMeshProUGUI tmp, string information)
        {
            tmp.text = $"{information}";
        }    

        /// <summary>
        /// 팝업 창이 열릴 경우, 우클릭된 캐릭터의 정보로 교체하는 함수.
        /// </summary>
        public void SetAllInformation()
        {
            // 팝업창 오픈 시, 현재 플레이어 컨트롤러를 찾아서 담기.
            playerController = FindObjectOfType<PlayerController>();
            // 해당 컨트롤러에서 현재 집중중인 캐릭터를 따로 저장.
            var focusCharacter = playerController.currentFocusCharacter;

            SetInformation(nickName, focusCharacter.boCharacter.nickName);
            SetInformation(job, focusCharacter.boCharacter.sdCharacter.job);
            SetInformation(maxHp, focusCharacter.boCharacter.maxHp.ToString());
            SetInformation(evade, focusCharacter.boCharacter.currentEvade.ToString());
            SetInformation(def, focusCharacter.boCharacter.currentDef.ToString());
            SetInformation(speed, focusCharacter.boCharacter.currentSpeed.ToString());
            SetInformation(accuracy, focusCharacter.boCharacter.curAccuracyCorrect.ToString());
            SetInformation(crit, focusCharacter.boCharacter.currentCritical.ToString());
            SetInformation(damage, $"{focusCharacter.boCharacter.currentMinATK} - {focusCharacter.boCharacter.currentMaxATK}");
            SetInformation(stunCount, focusCharacter.boCharacter.currentStunResist.ToString());
            SetInformation(addictedCount, focusCharacter.boCharacter.currentAddictedResist.ToString());
            SetInformation(diseaseCount, focusCharacter.boCharacter.currentDiseaseResist.ToString());
            SetInformation(deathDoorCount, focusCharacter.boCharacter.currentDeathResist.ToString());
            SetInformation(movingCount, focusCharacter.boCharacter.currentMovingResist.ToString());
            SetInformation(bleedingCount, focusCharacter.boCharacter.currentBleedingResist.ToString());
            SetInformation(weakeningCount, focusCharacter.boCharacter.currentWeakeningResist.ToString());
            SetInformation(trapDisarmCount, focusCharacter.boCharacter.currentTrapDisarm.ToString());
            
        }
    }
}