using Portfolio.BO;

namespace Portfolio.DataBase.Handler
{
    /// <summary>
    /// 로그인 시 필요한 데이터들을 서버에 요청하는 기능을 수행할 클래스
    /// </summary>
    public class LoginHandler
    {
        public LoginHandler() 
        {
            // 더미서버에 저장된 유저 DB
            var userData = DummyServer.Instance.userData;
            // 로그인 시, BoAccount에 유저 DB를 대입함
            GetAccountSuccess(userData);
        }

        /// <summary>
        /// 유저 데이터 요청 시, 실행할 함수.
        /// </summary>
        /// <param name="userData">BoAccount에 넣을 StaticData</param>
        public void GetAccountSuccess(UserData userData)
        {
            GameManager.User.boAccount = new BoAccount(userData);
        }
    }
}