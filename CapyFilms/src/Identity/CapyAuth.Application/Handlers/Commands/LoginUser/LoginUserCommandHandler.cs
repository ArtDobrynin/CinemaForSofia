using Capy.Common.Interfaces;
using Capy.Domain.Models.Auth;
using CapyAuth.Application.Models.Auth.Login;
using CapyAuth.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CapyAuth.Application.Handlers.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly ICapyDbContext _dbContext;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IAuthService _authService;
        public LoginUserCommandHandler(ICapyDbContext dbContext, IPasswordHasher<User> hasher, IAuthService authService)
        {
            _dbContext = dbContext;
            _hasher = hasher;
            _authService = authService;
        }

        public async Task<LoginUserResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == command.Login, cancellationToken);

            if (user == null)
            {
                return new LoginUserResult
                {
                    Token = null,
                    CreatedAt = DateTime.UtcNow,
                    MessageError = "There is no such user"
                };
            }

            var verificationPassword = _hasher.VerifyHashedPassword(user, user.HashPassword, command.Password);
            if(verificationPassword == PasswordVerificationResult.Failed)
            {
                return new LoginUserResult
                {
                    Token = null,
                    CreatedAt = DateTime.UtcNow,
                    MessageError = "Incorrect password"
                };
            }

            var token = _authService.GenerateSecurityToken(user.Id, user.Email, user.Login);

            return new LoginUserResult
            {
                UserId = user.Id,
                Token = token,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
