using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapyFilms.Application.Models.Cinema.GetNewFilms
{
    public class GetFilmsResult
    {
        public IEnumerable<GetNewFilmsItem> Data { get; set; } 
        public DateTime CreatedAt { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class GetNewFilmsItem
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string PosterUrl { get; set; }
    }
}
