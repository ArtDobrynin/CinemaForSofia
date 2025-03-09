using Capy.Common.Options;
using Capy.Domain.Enumerators;
using CapyAuth.Infrastructure.Interfaces;
using CapyFilms.Application.Handlers.Queries.GetNewCinema;
using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CapyFilms.Application.Handlers.Queries.GetCinemaByGenre
{
    public class GetCinemaByGenreQueryHandler : IRequestHandler<GetCinemaByGenreQuery, GetFilmsResult>
    {
        private readonly ICapyDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly KinopoiskCredentials _options;
        public GetCinemaByGenreQueryHandler(ICapyDbContext dbContext, HttpClient client, IOptions<KinopoiskCredentials> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
            _client = client;
        }

        public async Task<GetFilmsResult> Handle(GetCinemaByGenreQuery request, CancellationToken cancellationToken)
        {
            var idGenre = Enum.Parse<Genres>(request.genre);
            
            _client.DefaultRequestHeaders.Add("X-API-KEY", _options.SecurityKey);

            var response = await _client.GetAsync($"https://kinopoiskapiunofficial.tech/api/v2.2/films?order=RATING&genres={(int)idGenre}");
            if (!response.IsSuccessStatusCode)
            {
                var badResult = new GetFilmsResult
                {
                    Data = null,
                    CreatedAt = DateTime.Now,
                    ErrorMessage = $"Failed to retrieve product. Status code: {response.StatusCode}"
                };

                return badResult;
            }

            var stream = response.Content.ReadAsStream();
            var newFilms = JsonSerializer.Deserialize<NewFilmsItem>(stream);

            var result = new GetFilmsResult
            {
                Data = newFilms.items.Take(20).Select(e => new GetNewFilmsItem
                {
                    Id = e.KinopoiskId,
                    Name = e.NameRu ?? e.NameOriginal,
                    Status = _dbContext.GetFilmsToStatus(e.KinopoiskId),
                    PosterUrl = e.PosterUrl
                }),

                CreatedAt = DateTime.Now,
                ErrorMessage = string.Empty
            };

            return result;
        }
    }
}
