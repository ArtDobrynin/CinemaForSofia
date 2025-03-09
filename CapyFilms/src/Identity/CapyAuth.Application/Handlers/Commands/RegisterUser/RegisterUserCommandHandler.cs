using Capy.Common.Interfaces;
using Capy.Domain.Models.Auth;
using CapyAuth.Application.Models.Auth.Registration;
using CapyAuth.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CapyAuth.Application.Handlers.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly ICapyDbContext _dbContext;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher<User> _hasher;

        public RegisterUserCommandHandler(ICapyDbContext dbContext, IAuthService authService, IPasswordHasher<User> hasher)
        {
            _dbContext = dbContext;
            _authService = authService;
            _hasher = hasher;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var existsLoginOrEmail = (await _dbContext.Users
                                             .Select(e =>
                                             new
                                             {
                                                 IsEmailExists = _dbContext.Users.Any(e => e.Email == command.Email),
                                                 IsLoginExists = _dbContext.Users.Any(e => e.Login == command.Login)
                                             })
                                             .FirstOrDefaultAsync()) ?? new
                                             {
                                                 IsEmailExists = false,
                                                 IsLoginExists = false
                                             };

            if (existsLoginOrEmail.IsEmailExists)
            {
                var resultExistsEmail = new RegisterUserResult
                {
                    Token = null,
                    CreatedAt = DateTime.UtcNow,
                    ErrorMessage = "This email already exists"
                };
                return resultExistsEmail;
            }

            if (existsLoginOrEmail.IsLoginExists)
            {
                var resultExistsLogin = new RegisterUserResult
                {
                    Token = null,
                    CreatedAt = DateTime.UtcNow,
                    ErrorMessage = "This login already exists"
                };
                return resultExistsLogin;
            }

            var user = new User
            {
                Email = command.Email,
                Login = command.Login,
            };

            var passwordHasher = _hasher.HashPassword(user, command.Password);
            user.HashPassword = passwordHasher;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var token = _authService.GenerateSecurityToken(user.Id, user.Email, user.Login);
            var result = new RegisterUserResult
            {
                UserId = user.Id,
                Token = token,
                CreatedAt = DateTime.UtcNow,
            };

            return result;
        }
    }
}
