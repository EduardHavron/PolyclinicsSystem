using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Appointment;

namespace PolyclinicsSystemBackend.Validators.Appointment
{
    public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
    {
        public AppointmentDtoValidator()
        {
            RuleFor(dto => dto.DoctorId).NotEmpty();

            RuleFor(dto => dto.PatientId).NotEmpty();

            RuleFor(dto => dto.AppointmentDate).NotEmpty();
        }
    }
}