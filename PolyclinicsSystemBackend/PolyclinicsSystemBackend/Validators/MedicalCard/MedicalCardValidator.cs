using FluentValidation;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class MedicalCardValidator : AbstractValidator<Data.Entities.MedicalCard.MedicalCard>
    {
        public MedicalCardValidator()
        {
            RuleFor(dto => dto.Diagnoses).NotNull().Must(list => list.Count > 0)
                .WithMessage("Medical card should contain at least one diagnose");
        }
    }
}