using Capy.Common.Contracts.Cinema;
using Capy.Common.Contracts.Cinema.GetNewCinema;
using Capy.Common.Contracts.RandomizerFilms;
using CapyFilms.Application.Handlers.Commands.AddBookmarks;
using CapyFilms.Application.Handlers.Commands.NewFolder;
using CapyFilms.Application.Handlers.Commands.RandomizerFilms;
using CapyFilms.Application.Handlers.Queries.GetBookmarksCinema;
using CapyFilms.Application.Handlers.Queries.GetCinemaByGenre;
using CapyFilms.Application.Handlers.Queries.GetNewCinema;
using CapyFilms.Application.Handlers.Queries.GetSearchCinema;
using CapyFilms.Application.Handlers.Queries.GetWatchedFilms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CapyFilms.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CinemaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<GetNewCinemaResponse>> GetNewCinema(CancellationToken cancellationToken)
        {
            var films = await _mediator.Send(new GetNewFilmsQuery(), cancellationToken);

            if (!films.Data.Any())
            {
                var badResult = new GetNewCinemaResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.ErrorMessage
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                Data = films.Data.Select(e => new GetNewFilmsItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    PosterUrl = e.PosterUrl
                }),
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("genre")]
        public async Task<ActionResult<GetNewCinemaResponse>> GetCinemaByGenre(string genreName, CancellationToken cancellationToken)
        {
            var films = await _mediator.Send(new GetCinemaByGenreQuery(genreName), cancellationToken);
            if (!films.Data.Any())
            {
                var badResult = new GetNewCinemaResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.ErrorMessage
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                Data = films.Data.Select(e => new GetNewFilmsItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    PosterUrl = e.PosterUrl
                }),
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<GetNewCinemaResponse>> GetSearchFilms(string stringSearch, CancellationToken cancellationToken)
        {
            var films = await _mediator.Send(new GetSearchCinemaQuery(stringSearch), cancellationToken);
            if (!films.Data.Any())
            {
                var badResult = new GetNewCinemaResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.ErrorMessage
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                Data = films.Data.Select(e => new GetNewFilmsItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    PosterUrl = e.PosterUrl
                }),
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPost("bookmarks")]
        public async Task<ActionResult<SuccessResponse>> AddBookmark(int idCinema, CancellationToken cancellationToken)
        {
            var idUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idUser is null)
            {
                var badResult = new SuccessResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Error user is not auth"
                };

                return BadRequest(badResult);
            }


            var films = await _mediator.Send(new AddBookmarksCommand(idCinema, new Guid(idUser.Value)), cancellationToken);
            if (!films.Success)
            {
                var badResult = new SuccessResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.Message
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("bookmarksList")]
        public async Task<ActionResult<SuccessResponse>> GetBookmarks(CancellationToken cancellationToken)
        {
            var idUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idUser is null)
            {
                var badResult = new SuccessResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Error user is not auth"
                };

                return BadRequest(badResult);
            }


            var films = await _mediator.Send(new GetBookmarksCinemaQuery(new Guid(idUser.Value)), cancellationToken);
            if (!films.Data.Any())
            {
                var badResult = new GetNewCinemaResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.ErrorMessage
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                Data = films.Data.Select(e => new GetNewFilmsItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    PosterUrl = e.PosterUrl
                }),
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPost("watched")]
        public async Task<ActionResult<SuccessResponse>> AddWatchedFilm(int idCinema, CancellationToken cancellationToken)
        {
            var idUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idUser is null)
            {
                var badResult = new SuccessResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Error user is not auth"
                };

                return BadRequest(badResult);
            }


            var films = await _mediator.Send(new AddWatchedFilmCommand(idCinema, new Guid(idUser.Value)), cancellationToken);
            if (!films.Success)
            {
                var badResult = new SuccessResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.Message
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpGet("watchedList")]
        public async Task<ActionResult<SuccessResponse>> GetWatchedFilms(CancellationToken cancellationToken)
        {
            var idUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idUser is null)
            {
                var badResult = new SuccessResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Error user is not auth"
                };

                return BadRequest(badResult);
            }


            var films = await _mediator.Send(new GetWatchedFilmsQuery(new Guid(idUser.Value)), cancellationToken);
            if (!films.Data.Any())
            {
                var badResult = new GetNewCinemaResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.ErrorMessage
                };

                return BadRequest(badResult);
            }

            var result = new GetNewCinemaResponse
            {
                Data = films.Data.Select(e => new GetNewFilmsItem
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    PosterUrl = e.PosterUrl
                }),
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPost("random")]
        public async Task<ActionResult<GetNewCinemaResponse>> GetRandomFilm([FromBody] RandomFilmsItem randomFilm, CancellationToken cancellationToken)
        {
            var films = await _mediator.Send(new RandomizerFilmsCommand(randomFilm), cancellationToken);
            if (films.Data is null)
            {
                var badResult = new GetRandomCinemaResponse
                {
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = films.ErrorMessage
                };

                return BadRequest(badResult);
            }

            var result = new GetRandomCinemaResponse
            {
                Data = new GetNewFilmsItem
                {
                    Id = films.Data.Id,
                    Name = films.Data.Name,
                    Status = films.Data.Status,
                    PosterUrl = films.Data.PosterUrl
                },
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = string.Empty
            };

            return Ok(result);
        }
    }
}
