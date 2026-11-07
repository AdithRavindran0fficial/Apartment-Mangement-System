using FluentValidation;

namespace Nanma.Application.Features.Auth.Contracts;

public class SetMpinRequestValidator : AbstractValidator<SetMpinRequest>
{
    public SetMpinRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Mpin).NotEmpty().MinimumLength(4).MaximumLength(10);
        RuleFor(x => x.ConfirmMpin).NotEmpty().MinimumLength(4).MaximumLength(10);
    }
}
