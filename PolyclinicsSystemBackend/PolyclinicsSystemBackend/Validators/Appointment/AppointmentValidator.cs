using FluentValidation;

namespace PolyclinicsSystemBackend.Validators.Appointment
{
    public class AppointmentValidator : AbstractValidator<Data.Entities.Appointment.Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(dto => dto.DoctorId).NotEmpty()
                .WithMessage("Doctor must be chosen for appointment");

            RuleFor(dto => dto.PatientId).NotEmpty()
                .WithMessage("Patient must be chosen for appointment");

            RuleFor(dto => dto.AppointmentDate).NotEmpty()
                .WithMessage("Appointment date must be chosen for appointment");
        }
    }
}