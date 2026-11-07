using FluentValidation;

namespace Nanma.Application.Features.Auth.Contracts;

public class LoginWithMpinRequestValidator : AbstractValidator<LoginWithMpinRequest>
{
    public LoginWithMpinRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Mpin).NotEmpty().MinimumLength(4).MaximumLength(10);
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(100);
    }
}
