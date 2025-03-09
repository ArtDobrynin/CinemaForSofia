using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;

namespace CapyFilms.Application.Handlers.Queries.GetCinemaByGenre
{
    public record GetCinemaByGenreQuery(string genre) : IRequest<GetFilmsResult>;
}
