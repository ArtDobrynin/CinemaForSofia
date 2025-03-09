using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;

namespace CapyFilms.Application.Handlers.Queries.GetBookmarksCinema
{
    public record GetBookmarksCinemaQuery(Guid idUser) : IRequest<GetFilmsResult>;
}
