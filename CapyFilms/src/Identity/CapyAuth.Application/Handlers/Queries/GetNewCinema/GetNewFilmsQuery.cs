using CapyFilms.Application.Models.Cinema.GetNewFilms;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapyFilms.Application.Handlers.Queries.GetNewCinema
{
    public record GetNewFilmsQuery() : IRequest<GetFilmsResult>; 
}
