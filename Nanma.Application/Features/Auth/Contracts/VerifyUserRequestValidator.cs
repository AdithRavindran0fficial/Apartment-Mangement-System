using FluentValidation;

namespace Nanma.Application.Features.Auth.Contracts;

public class VerifyUserRequestValidator : AbstractValidator<VerifyUserRequest>
{
    public VerifyUserRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
    }
}
