using FluentValidation;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class TreatmentValidator : AbstractValidator<Treatment>
    {
        public TreatmentValidator()
        {
            RuleFor(dto => dto.TreatmentInstructions).NotNull().Length(10, 500000)
                .WithMessage("Treatment instructions should be at least 10 chars long");
        }
    }
}