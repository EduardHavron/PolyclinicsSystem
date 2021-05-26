using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;

namespace PolyclinicsSystemBackend.Validators.Authorize
{
    public class AuthorizeDtoValidator : AbstractValidator<AuthorizeDto>
    {
        public AuthorizeDtoValidator()
        {
            RuleFor(dto => dto.Email).EmailAddress()
                .WithMessage("Email should be not empty and valid");
            RuleFor(dto => dto.Password).NotEmpty().Length(4, 50)
                .WithMessage("Password should be between 4 and 50 chars");
        }
    }
}