namespace Nanma.Domain.Exceptions;

public abstract class AppException(string errorCode, string message) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}
