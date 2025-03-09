using CapyAuth.Application.Models.Auth.Login;
using MediatR;

namespace CapyAuth.Application.Handlers.Commands.LoginUser
{
    public record LoginUserCommand
        (
            string Login,
            string Password
        ) : IRequest<LoginUserResult>;
}
