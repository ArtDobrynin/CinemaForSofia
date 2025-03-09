using Capy.Common.Options;
using Capy.Domain.Models.Auth;
using Capy.Domain.Models.Cinema;
using CapyAuth.Infrastructure.Interfaces;
using CapyFilms.Application.Models.Cinema;
using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CapyFilms.Application.Handlers.Commands.AddBookmarks
{
    public class AddBookmarksCommandHandler : IRequestHandler<AddBookmarksCommand, SuccessResult>
    {
        private readonly HttpClient _client;
        private readonly KinopoiskCredentials _options;
        private readonly ICapyDbContext _dbContext;
        public AddBookmarksCommandHandler(HttpClient client, IOptions<KinopoiskCredentials> options, ICapyDbContext dbContext)
        {
            _client = client;
            _options = options.Value;
            _dbContext = dbContext;
        }

        public async Task<SuccessResult> Handle(AddBookmarksCommand request, CancellationToken cancellationToken)
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

            var newFilms = new Films
            {
                Id = Guid.NewGuid(),
                KinopoiskId = result.KinopoiskId,
                Name = result.NameRu ?? result.NameOriginal,
                PosterUrl = result.PosterUrl,
                Status = false
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
