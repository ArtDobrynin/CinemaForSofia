using Capy.Common.Contracts.RandomizerFilms;
using CapyFilms.Application.Models.Cinema.RandomFilm;
using MediatR;

namespace CapyFilms.Application.Handlers.Commands.RandomizerFilms
{
    public record RandomizerFilmsCommand(RandomFilmsItem data) : IRequest<RandomFilmResult>;
}
