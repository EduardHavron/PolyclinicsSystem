using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Account.Authorize;
using PolyclinicsSystemBackend.Dtos.Account.Register;
using PolyclinicsSystemBackend.HelperEntities;

namespace PolyclinicsSystemBackend.Validators.Authorize
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty().EmailAddress();

            RuleFor(dto => dto.Password).NotEmpty().Length(4, 50);

            RuleFor(dto => dto.FirstName).NotEmpty().Length(2, 20);

            RuleFor(dto => dto.LastName).NotEmpty().Length(2, 20);

            RuleFor(dto => dto.Role).IsInEnum();

            RuleFor(dto => dto.DoctorType).NotEmpty()
                .When(dto => dto.Role == Roles.Doctor);
        }
    }
}