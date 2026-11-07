namespace Nanma.Domain.Exceptions;

public sealed class NotFoundException(string message, string errorCode = "not_found")
    : AppException(errorCode, message);
