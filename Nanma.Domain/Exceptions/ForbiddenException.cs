namespace Nanma.Domain.Exceptions;

public sealed class ForbiddenException(string message, string errorCode = "forbidden")
    : AppException(errorCode, message);
