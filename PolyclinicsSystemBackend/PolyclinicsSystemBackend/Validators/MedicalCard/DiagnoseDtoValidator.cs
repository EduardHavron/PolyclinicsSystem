using FluentValidation;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class DiagnoseDtoValidator : AbstractValidator<DiagnoseDto>
    {
        public DiagnoseDtoValidator()
        {
            RuleFor(dto => dto.DiagnoseInfo).NotNull().Length(10, 50000);
        }
    }
}