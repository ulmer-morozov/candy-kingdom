namespace CandyKingdom.Marcy.Skeleton;

public interface IHaveDataRouteWithData<T> : IHaveDataRoute
    where T : class
{
    public T Data { get; }
}
