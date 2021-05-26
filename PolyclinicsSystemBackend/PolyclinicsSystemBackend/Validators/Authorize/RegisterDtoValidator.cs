using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;

namespace PolyclinicsSystemBackend.Validators.Authorize
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty().EmailAddress()
                .WithMessage("Email should be between not empty and valid");

            RuleFor(dto => dto.Password).NotEmpty().Length(4, 50)
                .WithMessage("Password should be between 4 and 50 chars long");

            RuleFor(dto => dto.FirstName).NotEmpty().Length(2, 20)
                .WithMessage("First name should be between 2 and 20 chars long");

            RuleFor(dto => dto.LastName).NotEmpty().Length(2, 20)
                .WithMessage("Last name should be between 2 and 20 chars long");
        }
    }
}