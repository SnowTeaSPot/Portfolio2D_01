namespace Portfolio.Util
{
    /// <summary>
    /// 해당 인터페이스를 가지고 있는 클래스는, 풀링 가능한 오브젝트로 분류됨.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// 오브젝트가 재사용될 수 있는 상태인지를 나타내는 프로퍼티.
        /// </summary>
        bool CanRecycle { get; set; }
    }
}