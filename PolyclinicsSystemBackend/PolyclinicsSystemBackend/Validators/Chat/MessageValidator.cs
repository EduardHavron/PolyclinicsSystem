using FluentValidation;
using PolyclinicsSystemBackend.Dtos.Chat;

namespace PolyclinicsSystemBackend.Validators.Chat
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(dto => dto.SenderId).NotEmpty()
                .WithMessage("Sender should be specified");
            RuleFor(dto => dto.MessageContent).NotEmpty()
                .WithMessage("Message should be not empty");
        }
    }
}