using Capy.Common.Options;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CapyAuth.Application.Handlers.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly PathToValidateText _options;

        public RegisterUserCommandValidator(IOptions<PathToValidateText> options)
        {
            _options = options.Value;

            RuleFor(f => f.Email).NotEmpty().EmailAddress();
            RuleFor(f => f.Login).NotEmpty().Length(6, 24);
            RuleFor(f => f.Password).NotEmpty().Length(8, 12);
            RuleFor(f => f.Password).Must(p => !GetText(_options.PathValidateText).Contains(p));
        }

        private Func<string, string> GetText = (string path) =>
        {
            using (StreamReader stream = new StreamReader(path))
            {
                return stream.ReadToEnd();
            }
        };
    }
}
