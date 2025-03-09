using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;

namespace CapyFilms.Application.Handlers.Queries.GetSearchCinema
{
    public record GetSearchCinemaQuery(string stringSearch) : IRequest<GetFilmsResult>;
}
