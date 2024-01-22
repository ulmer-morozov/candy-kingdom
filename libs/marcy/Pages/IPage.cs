namespace CandyKingdom.Marcy.Pages;

public interface IPage<out TData>
    where TData : PageData
{
    TData Data { get; }
}
