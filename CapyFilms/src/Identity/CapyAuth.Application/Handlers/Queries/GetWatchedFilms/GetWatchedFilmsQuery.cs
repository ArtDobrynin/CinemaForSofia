using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;

namespace CapyFilms.Application.Handlers.Queries.GetWatchedFilms
{
    public record GetWatchedFilmsQuery(Guid idUser) : IRequest<GetFilmsResult>;
}
