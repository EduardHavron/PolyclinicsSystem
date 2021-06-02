using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;

namespace PolyclinicsSystemBackend.Validators.Authorize
{
    public class AuthorizeDtoValidator : AbstractValidator<AuthorizeDto>
    {
        public AuthorizeDtoValidator()
        {
            RuleFor(dto => dto.Email).EmailAddress();

            RuleFor(dto => dto.Password).NotEmpty().Length(4, 50);
        }
    }
}