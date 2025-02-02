using UnityEngine;
using System;

namespace Portfolio.SD
{
    // ScriptableObject :
    // 유니티에서 지원하는 데이터와 정적 메서드만을 갖는 클래스.
    // 스크립터블 오브젝트를 상속받는 클래스는 일반적인 게임 오브젝트로 인스턴스가 불가능하다.
    public class StaticData : ScriptableObject
    {
        // 각각의 기획 데이터를 구분하기 위한 키 값.
        //  -> index는 해당 기획데이터에서 유일하게 존재해야 한다.
        public int index;
    }
}
