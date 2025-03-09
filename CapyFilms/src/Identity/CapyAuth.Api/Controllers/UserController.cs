using Capy.Common.Contracts.LoginUser;
using Capy.Common.Contracts.RegisterUser;
using CapyAuth.Application.Handlers.Commands.LoginUser;
using CapyAuth.Application.Handlers.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CapyAuth.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserRequest item, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new RegisterUserCommand(item.Email, item.Login, item.Password), cancellationToken);

            if (result.Token is null)
            {
                var badResponse = new RegisterUserResponse
                {
                    Data = new RegisterUserItem
                    {
                        Token = result.Token,
                        CreatedAt = DateTime.UtcNow
                    },
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = result.ErrorMessage
                };
                return BadRequest(badResponse);
            }

            var response = new RegisterUserResponse
            {
                Data = new RegisterUserItem
                {
                    Token = result.Token,
                    CreatedAt = DateTime.UtcNow
                },
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = null
            };


            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<LoginUserResponse>> LoginUser([FromBody] LoginUserRequest item, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new LoginUserCommand(item.Login, item.Password), cancellationToken);

            if (result.Token is null)
            {
                var badResponse = new LoginUserResponse
                {
                    Data = new LoginUserItem
                    {
                        Token = result.Token,
                        CreatedAt = DateTime.UtcNow
                    },
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = result.MessageError
                };

                return BadRequest(badResponse);
            }

            var response = new LoginUserResponse
            {
                Data = new LoginUserItem
                {
                    UserId = result.UserId,
                    Token = result.Token,
                    CreatedAt = DateTime.UtcNow
                },
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = null
            };

            return Ok(response);
        }
    }
}
