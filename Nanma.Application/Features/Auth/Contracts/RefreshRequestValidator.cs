using FluentValidation;

namespace Nanma.Application.Features.Auth.Contracts;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(100);
    }
}
