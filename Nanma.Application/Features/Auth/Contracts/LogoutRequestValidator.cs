using FluentValidation;

namespace Nanma.Application.Features.Auth.Contracts;

public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
{
    public LogoutRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(100);
    }
}
