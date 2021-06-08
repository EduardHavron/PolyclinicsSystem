using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Appointment;

namespace PolyclinicsSystemBackend.Validators.Appointment
{
    public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
    {
        public AppointmentDtoValidator()
        {
            RuleFor(dto => dto.Doctor).NotNull();

            RuleFor(dto => dto.Patient).NotNull();

            RuleFor(dto => dto.AppointmentDate).NotEmpty();
        }
    }
}