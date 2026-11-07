namespace Nanma.Domain.Exceptions;

public sealed class ConflictException(string message, string errorCode = "conflict")
    : AppException(errorCode, message);
