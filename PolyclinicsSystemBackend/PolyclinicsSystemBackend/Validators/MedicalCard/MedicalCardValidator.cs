using FluentValidation;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class MedicalCardValidator : AbstractValidator<Data.Entities.MedicalCard.MedicalCard>
    {
        public MedicalCardValidator()
        {
            // RuleFor(dto => dto.Diagnoses).NotEmpty().Must(list => list.Count > 0)
            //     .WithMessage("Medical card should contain at least one diagnose");
            RuleFor(dto => dto.Gender).NotEmpty().IsInEnum()
                .WithMessage("Unknown gender in medical card");
            RuleFor(dto => dto.Height).NotEmpty().InclusiveBetween(60, 270)
                .WithMessage("Your height should be in inclusive range between 60 and 270");
            RuleFor(dto => dto.Weight).NotEmpty().InclusiveBetween(30, 180)
                .WithMessage("Your weight should be in inclusive range between 30 and 180");
            RuleFor(dto => dto.Age).NotEmpty().InclusiveBetween(4, 130)
                .WithMessage("Your age should be in inclusive range between 4 and 130");
        }
    }
}