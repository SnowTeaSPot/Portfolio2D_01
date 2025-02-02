using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace Portfolio.SD
{
    /// <summary>
    /// 모든 기획 데이터를 들고 있을 클래스
    /// 데이터를 로드하고 들고 있기만 할 것이므로, 모노를 상속 받지 않는다.
    /// </summary>
    // Serializable : Mono를 갖지 않는 일반 C# 클래스를 인스펙터에 노출시키기 위한 직렬화.
    [Serializable]
    public class StaticDataModule
    {
        //SD데이터 폴더로 만든 변수들.
        public List<SDCharacter> sdCharacters;
        public List<SDStage> sdstages;
        public List<SDEnemy> sdEnemy;

        //Json파일로 만든 SD데이터들.
        public List<SDEnemyTile> sdEnemyTile;

        /// <summary>
        /// 아직 쓸일 없음. 기획 데이터 초기화 용도임.
        /// 
        /// 이제 씀 ㅋ
        /// </summary>
        public void Initialize()
        {
            var loader = new StaticDataLoader();

            loader.Load(out sdEnemyTile);
        }

        private class StaticDataLoader
        {
            private string path;
            public StaticDataLoader()
            {
                path = $"{Application.dataPath}/Resources/StaticData/Json";
            }

            /// <summary>
            /// 기획 데이터 json을 읽어와 T타입 데이터 리스트로 파생해주는 기능.
            /// </summary>
            /// <typeparam name="T">변환하고자 하는 타입.</typeparam>
            /// <param name="data">변환된 T타입의 데이터들을 담을 리스트</param>
            public void Load<T>(out List<T> data) where T : StaticData
            {
                // json 파일이름을 T타입 이름을 통해서 구하기.
                // 이때 모든 기획데이터는 SD 라는 접두어로 시작하기 때문에 SD 접두사만 지우면, json 파일 이름과 동일해짐.
                var fileName = typeof(T).Name.Remove(0, "SD".Length);

                // json 파일 읽어들이기.
                var json = File.ReadAllText($"{path}/{fileName}.json");

                // 파라미터 data를 List<T>가 아닌 out List<T>로 선언한 이유는?
                // List는 참조 타입이므로, 전달 시 어떤 데이터를 할당해도 그대로 유지 된다고 생각할 수 있으나, 사실 아님.

                // 참조타입의 객체의 필드에 접근하여 데이터 할당은 가능함.
                // 그러나, 전달받은 참조 자체를 할당하는 작업은 out키워드가 없으면 불가능하다.
                // 실제로 out 키워드를 빼고 data에 리스트를 할당하면 그대로 전달이 안됨.(유지가 안된다.)

                //읽어온 json은 T타입 리스트 형태로 바꿔주기.
                data = JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
    }
}
