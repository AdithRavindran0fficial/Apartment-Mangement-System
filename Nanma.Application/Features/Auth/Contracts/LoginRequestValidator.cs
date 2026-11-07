using FluentValidation;

namespace Nanma.Application.Features.Auth.Contracts;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(100);
    }
}
