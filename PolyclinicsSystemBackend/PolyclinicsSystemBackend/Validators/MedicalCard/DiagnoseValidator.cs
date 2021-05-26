using FluentValidation;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class DiagnoseValidator : AbstractValidator<Diagnose>
    {
        public DiagnoseValidator()
        {
            RuleFor(dto => dto.DiagnoseInfo).NotNull().Length(10, 50000)
                .WithMessage("Diagnose should be at least 10 characters long");
            RuleFor(dto => dto.MedicalCardId).GreaterThan(0)
                .WithMessage("Diagnose cannot exist without medical card");
        }
    }
}