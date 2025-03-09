using CapyFilms.Application.Models.Cinema;
using MediatR;

namespace CapyFilms.Application.Handlers.Commands.NewFolder
{
    public record AddWatchedFilmCommand(int idCinema, Guid idUser) : IRequest<SuccessResult>;
}
