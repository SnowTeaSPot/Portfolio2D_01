using Portfolio.UI;
using Portfolio.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    using Actor = Portfolio.Object.Actor;

    /// <summary>
    ///  인게임 내 활성화된 객체들의 컬렉션을 갖고 제어하는 클래스.
    /// </summary>
    public class IngameManager : Singleton<IngameManager>
    {
        private int idCount = 0;

        /// <summary>
        /// 현재 활성화된 액터들을 갖는 컬렉션
        /// </summary>
        public List<Actor> Characters { get; set; } = new List<Actor>();

        //왜 항상 사람들은 Enemys로 쓰는 걸까 Enemies인데...
        public List<Actor> Enemys { get; set; } = new List<Actor>();

        private void FixedUpdate()
        {
            ActorUpdate(Characters);
            ActorUpdate(Enemys);
        }
        public void AddActor(Actor actor)
        {
            actor.id = ++idCount;

            switch (actor.boActor.type)
            {
                case Define.ObjectType.Character:
                    Characters.Add(actor);
                    // 캐릭터에게 체력과 스트레스 바를 추가.
                    UIWindowManager.Instance.GetWindow<UIIngame>().AddHpBar(actor);
                    UIWindowManager.Instance.GetWindow<UIIngame>().AddStressBar(actor);
                    break;
                case Define.ObjectType.Enemy:
                    Enemys.Add(actor);
                    UIWindowManager.Instance.GetWindow<UIIngame>().AddHpBar(actor);
                    break;
                case Define.ObjectType.Ground:
                    break;
                case Define.ObjectType.Door:
                    break;
                case Define.ObjectType.Accessible:
                    break;
                case Define.ObjectType.Trap:
                    break;
            }
        }

        /// <summary>
        /// actors의 전용 업데이트
        /// </summary>
        /// <param name="actors">해당하는 컬렉션</param>
        private void ActorUpdate(List<Actor> actors)
        {
            for(int i = 0; i < actors.Count; i++)
            {
                // 액터가 죽지 않았을 경우.
                if (actors[i].State != Define.State.Dead) 
                {
                    //Actor 내부에 있는 전용 업데이트 함수를 실행.
                    actors[i].Execute();
                }
                // 액터가 죽었을 경우.
                else
                {
                    //해당 액터를 컬렉션에서 제거.
                    actors.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}