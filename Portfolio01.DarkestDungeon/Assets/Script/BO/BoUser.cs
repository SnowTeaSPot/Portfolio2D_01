using Portfolio.BO;
using System;
using System.Collections.Generic;

namespace Portfolio.Bo
{
    /// <summary>
    /// 게임매니저에 등록될 유저가 가지고 있을 데이터.
    /// 지금은 BoAccount밖에 없음. (애초에 여기다가 다 집어넣었다.)
    /// </summary>
    [Serializable]
    public class BoUser
    {
        public BoAccount boAccount;
    }
}
