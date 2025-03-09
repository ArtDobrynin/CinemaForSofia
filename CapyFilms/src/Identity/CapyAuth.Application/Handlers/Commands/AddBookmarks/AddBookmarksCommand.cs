using CapyFilms.Application.Models.Cinema;
using MediatR;

namespace CapyFilms.Application.Handlers.Commands.AddBookmarks
{
    public record AddBookmarksCommand(int idCinema, Guid idUser) : IRequest<SuccessResult>;
}
