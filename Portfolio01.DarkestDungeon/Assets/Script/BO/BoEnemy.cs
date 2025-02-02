using Portfolio.SD;
using System;

namespace Portfolio.BO
{
    [Serializable]
    public class BoEnemy : BoActor
    {
        public SDEnemy sdEnemy;

        public BoEnemy(SDActor sdActor) : base(sdActor)
        {
            sdEnemy = sdActor as SDEnemy;
        }
    }
}