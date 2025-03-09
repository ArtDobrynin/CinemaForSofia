using CapyAuth.Infrastructure.Interfaces;
using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CapyFilms.Application.Handlers.Queries.GetWatchedFilms
{
    public class GetWatchedFilmsQueryHandler : IRequestHandler<GetWatchedFilmsQuery, GetFilmsResult>
    {
        private readonly ICapyDbContext _dbContext;
        public GetWatchedFilmsQueryHandler(ICapyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetFilmsResult> Handle(GetWatchedFilmsQuery request, CancellationToken cancellationToken)
        {
            var listFilms = await _dbContext.Users
                                            .Include(e => e.Films)
                                            .Where(u => u.Id == request.idUser)
                                            .SelectMany(f => f.Films).ToListAsync();

            var result = new GetFilmsResult
            {
                Data = listFilms.Where(e => e.Status).Select(e => new GetNewFilmsItem
                {
                    Id = e.KinopoiskId,
                    Name = e.Name,
                    Status = true,
                    PosterUrl = e.PosterUrl
                }),

                CreatedAt = DateTime.Now,
                ErrorMessage = string.Empty
            };

            return result;
        }
    }
}
