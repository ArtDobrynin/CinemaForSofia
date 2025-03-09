using CapyAuth.Application.Models.Auth.Registration;
using MediatR;

namespace CapyAuth.Application.Handlers.Commands.RegisterUser
{
    public record RegisterUserCommand
        (
            string Email,
            string Login,
            string Password
        ) : IRequest<RegisterUserResult>;
}
