namespace Restaurant.Api.Services;

public sealed record OperationResult<T>(T? Value, string? Error)
{
    public bool IsSuccess => Error is null;

    public static OperationResult<T> Success(T value) => new(value, null);

    public static OperationResult<T> Failure(string error) => new(default, error);
}
