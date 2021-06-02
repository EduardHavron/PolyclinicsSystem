using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Chat;

namespace PolyclinicsSystemBackend.Validators.Chat
{
    public class MessageDtoValidator : AbstractValidator<MessageDto>
    {
        public MessageDtoValidator()
        {
            RuleFor(dto => dto.SenderId).NotEmpty();
            
            RuleFor(dto => dto.MessageContent).NotEmpty();
        }
    }
}