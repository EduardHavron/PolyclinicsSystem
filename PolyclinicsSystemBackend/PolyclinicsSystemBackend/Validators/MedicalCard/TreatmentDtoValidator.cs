using FluentValidation;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class TreatmentDtoValidator : AbstractValidator<TreatmentDto>
    {
        public TreatmentDtoValidator()
        {
            RuleFor(dto => dto.TreatmentInstructions).NotNull().Length(10, 500000);
        }
    }
}