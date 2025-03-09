using Capy.Common.Options;
using Capy.Domain.Models.Cinema;
using CapyAuth.Infrastructure.Interfaces;
using CapyFilms.Application.Models.Cinema.GetNewFilms;
using CapyFilms.Application.Models.Cinema;
using MediatR;
using Microsoft.Extensions.Options;
using CapyFilms.Application.Handlers.Commands.NewFolder;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CapyFilms.Application.Handlers.Commands.AddWatchedFilm
{
    public class AddWatchedFilmCommandHandler : IRequestHandler<AddWatchedFilmCommand, SuccessResult>
    {
        private readonly HttpClient _client;
        private readonly KinopoiskCredentials _options;
        private readonly ICapyDbContext _dbContext;
        public AddWatchedFilmCommandHandler(HttpClient client, IOptions<KinopoiskCredentials> options, ICapyDbContext dbContext)
        {
            _client = client;
            _options = options.Value;
            _dbContext = dbContext;
        }

        public async Task<SuccessResult> Handle(AddWatchedFilmCommand request, CancellationToken cancellationToken)
        {
            _client.DefaultRequestHeaders.Add("X-API-KEY", _options.SecurityKey);

            var response = await _client.GetAsync($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{request.idCinema}");
            if (!response.IsSuccessStatusCode)
            {
                var badResult = new SuccessResult
                {
                    Success = false,
                    CreatedAt = DateTime.Now,
                    Message = $"Failed to retrieve product. Status code: {response.StatusCode}"
                };

                return badResult;
            }

            var stream = response.Content.ReadAsStream();
            var result = JsonSerializer.Deserialize<Film>(stream);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.idUser);
            if (user is null)
            {
                var badResult = new SuccessResult
                {
                    Success = false,
                    CreatedAt = DateTime.Now,
                    Message = $"User not found, sorry!"
                };

                return badResult;
            }

            if (_dbContext.Films.Any(f => f.KinopoiskId == result.KinopoiskId))
            {
                var anyFilms = await _dbContext.Films.FirstOrDefaultAsync(f => f.KinopoiskId == result.KinopoiskId);
                anyFilms.Status = !anyFilms.Status;
                _dbContext.Update(anyFilms);
                await _dbContext.SaveChangesAsync();

                return new SuccessResult
                {
                    Success = true, 
                    CreatedAt = DateTime.Now,
                    Message = string.Empty
                };
            }

            var newFilms = new Films
            {
                Id = Guid.NewGuid(),
                KinopoiskId = result.KinopoiskId,
                Name = result.NameRu ?? result.NameOriginal,
                PosterUrl = result.PosterUrl,
                Status = true
            };

            newFilms.Users.Add(user);

            await _dbContext.Films.AddAsync(newFilms);
            await _dbContext.SaveChangesAsync();

            return new SuccessResult
            {
                Success = true,
                CreatedAt = DateTime.Now,
                Message = string.Empty
            };
        }
    }
}
