using System.Diagnostics.CodeAnalysis;

namespace CandyKingdom.Marcy;

public sealed record ResultOrError<T> : ResultOrError
{
    public T? Data { get; }

    [MemberNotNullWhen(returnValue: true, member: nameof(Data))]
    public override bool IsSuccessful => Data != null;

    public ResultOrError(T? data, string message = "", int errorCode = 0)
      : base(data != null, message, errorCode)
    {
        Data = data;
    }
}

public record ResultOrError
{
    public virtual bool IsSuccessful { get; }
    public string Message { get; }
    public int ErrorCode { get; }

    protected ResultOrError(bool isSuccessful, string message = "", int errorCode = 0)
    {
        IsSuccessful = isSuccessful;
        Message = message;
        ErrorCode = errorCode;
    }

    public static ResultOrError Success()
    {
        return new(true);
    }

    public static ResultOrError<T> Success<T>(T data)
      where T : class
    {
        return new(data);
    }

    public static ResultOrError Fail(string message, int errorCode = 0)
    {
        return new(false, message, errorCode);
    }

    public static ResultOrError<T> Fail<T>(string message, int? errorCode = null)
      where T : class
    {
        return new(default, message, errorCode ?? 0);
    }
}
