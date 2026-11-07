namespace Nanma.Domain.Exceptions;

public sealed class UnauthorizedException(string message, string errorCode = "unauthorized")
    : AppException(errorCode, message);
