using Portfolio.Util;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Portfolio.DataBase
{
    /// <summary>
    /// 더미서버의 역할을 수행할 클래스.
    /// 더미서버에서 사용할 데이터베이스(DB)를 갖는다.
    /// </summary>
    public class DummyServer : Singleton<DummyServer>
    {
        /// <summary>
        /// 더미 서버에서 갖는 유저 데이터 (유저 DB)
        /// </summary>
        public UserData userData;
        /// <summary>
        /// 데이터 세이브를 실행할 코루틴 변수.
        /// </summary>
        private Coroutine saveCoroutine;

        public void Initialiaze()
        {

        }

        /// <summary>
        /// 더미 유저 데이터를 저장하는 기능.
        /// </summary>
        public void Save()
        {
            // DB 데이터는 스크립터블 오브젝트를 기반으로 작성되었기 때문에
            // 저장 기능은 에디터에서만 가능함.
            
            // 저장시킬 유저 데이터를 플래그로 설정하고
            // 유니티에서 런타임에 사용되는 (프리팹 또는 스크립터블 오브젝트) 등은
            // 일반적으로 변동사항을 원본에 저장하는 목적으로 사용되는 데이터가 아님.
            // 하지만, 런타임 시 사용되던 데이터를 저장하고 싶을 대, 디스크에서 쓸 수 있게
            // 플래그를 설정하면 가능함.
            
            // 세이브 코루틴이 null이 아닐 경우.
            if(saveCoroutine != null)
            { 
                // 해당 코루틴을 중지
                StopCoroutine(saveCoroutine);
                // 다음에 사용하기 위해 내부를 비움.
                saveCoroutine = null;
            }

            saveCoroutine = StartCoroutine(SaveProgress());

            IEnumerator SaveProgress()
            {
                // EditorUtility.SetDirty(저장하려는 데이터) : 에디터 상에서 에셋 파일을 변경한 경우, 변경사항을 저장하기 위해 사용되는 함수.
                EditorUtility.SetDirty(userData);
                // 유니티의 File -> Save Project와 똑같은 기능을 코드로 구현하는 함수.
                AssetDatabase.SaveAssets();

                yield return null;
            }
        }
    }
}
