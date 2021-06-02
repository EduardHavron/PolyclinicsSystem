using FluentValidation;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Validators.MedicalCard
{
    public class MedicalCardDtoValidator : AbstractValidator<MedicalCardDto>
    {
        public MedicalCardDtoValidator()
        {

            RuleFor(dto => dto.Gender).NotEmpty().IsInEnum();

            RuleFor(dto => dto.Height).NotEmpty().InclusiveBetween(60, 270);

            RuleFor(dto => dto.Weight).NotEmpty().InclusiveBetween(30, 180);

            RuleFor(dto => dto.Age).NotEmpty().InclusiveBetween(4, 130);
        }
    }
}