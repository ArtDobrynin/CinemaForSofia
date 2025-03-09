using Capy.Common.Options;
using Capy.Domain.Enumerators;
using CapyAuth.Infrastructure.Interfaces;
using CapyFilms.Application.Models.Cinema;
using CapyFilms.Application.Models.Cinema.GetNewFilms;
using CapyFilms.Application.Models.Cinema.RandomFilm;
using CapyFilms.Application.Models.People;
using MediatR;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;

namespace CapyFilms.Application.Handlers.Commands.RandomizerFilms
{
    public class RandomizerFilmsCommandHandler : IRequestHandler<RandomizerFilmsCommand, RandomFilmResult>
    {
        private readonly HttpClient _client;
        private readonly KinopoiskCredentials _options;
        private readonly ICapyDbContext _dbContext;
        public RandomizerFilmsCommandHandler(HttpClient client, IOptions<KinopoiskCredentials> options, ICapyDbContext dbContext)
        {
            _client = client;
            _options = options.Value;
            _dbContext = dbContext;
        }

        public async Task<RandomFilmResult> Handle(RandomizerFilmsCommand request, CancellationToken cancellationToken)
        {

            _client.DefaultRequestHeaders.Add("X-API-KEY", _options.MainKey);

            var link = "https://api.kinopoisk.dev/v1.4/movie/random?";

            if (request.data.Year is not null)
            {
                link += $"year={request.data.Year}";
            }

            if (request.data.Genre is not null)
            {
                link += $"genres.name={request.data.Genre.ToLower()}&";
            }

            if (request.data.Actor is not null)
            {
                var responseActor = await _client.GetAsync($"https://api.kinopoisk.dev/v1.4/person/search?query={request.data.Actor}");
                var streamActor = responseActor.Content.ReadAsStream();
                var idActor = JsonSerializer.Deserialize<PersonByNameResponse>(streamActor);

                if (idActor.Items.Any())
                {
                    link += $"persons.id={idActor.Items.First().Id}&persons.enProfession=actor";
                }
            }

            if (request.data.Producer is not null)
            {
                var responseProducer = await _client.GetAsync($"https://api.kinopoisk.dev/v1.4/person/search?query={request.data.Producer}");
                var streamProducer = responseProducer.Content.ReadAsStream();
                var idProducer = JsonSerializer.Deserialize<PersonByNameResponse>(streamProducer);

                if(idProducer.Items.Any())
                {
                    link += $"persons.id={idProducer.Items.First().Id}&persons.enProfession=director";
                }
            }

            var response = await _client.GetAsync(link);
            if (!response.IsSuccessStatusCode)
            {
                var badResult = new RandomFilmResult
                {
                    Data = null,
                    CreatedAt = DateTime.Now,
                    ErrorMessage = $"Failed to retrieve product. Status code: {response.StatusCode}"
                };

                return badResult;
            }

            var stream = response.Content.ReadAsStream();
            var movieId = JsonSerializer.Deserialize<MovieDtoV1_4>(stream);

            _client.DefaultRequestHeaders.Remove("X-API-KEY");
            _client.DefaultRequestHeaders.Add("X-API-KEY", _options.SecurityKey);

            if (movieId is null)
            {
                var badResult = new RandomFilmResult
                {
                    Data = null,
                    CreatedAt = DateTime.Now,
                    ErrorMessage = $"Failed to retrieve product. Status code: {response.StatusCode}"
                };

                return badResult;
            }

            var responseRandom = await _client.GetAsync($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{movieId.Id}");
            if (!responseRandom.IsSuccessStatusCode)
            {
                var badResult = new RandomFilmResult
                {
                    Data = null,
                    CreatedAt = DateTime.Now,
                    ErrorMessage = $"Failed to retrieve product. Status code: {response.StatusCode}"
                };

                return badResult;
            }

            var streamRandom = responseRandom.Content.ReadAsStream();
            var random = JsonSerializer.Deserialize<Film>(streamRandom);

            var result = new RandomFilmResult
            {
                Data = new RandomFilmItem
                {
                    Id = random.KinopoiskId,
                    Name = random.NameRu ?? random.NameOriginal,
                    Status = _dbContext.GetFilmsToStatus(random.KinopoiskId),
                    PosterUrl = random.PosterUrl
                },

                CreatedAt = DateTime.Now,
                ErrorMessage = string.Empty
            };

            return result;
        }
    }
}
