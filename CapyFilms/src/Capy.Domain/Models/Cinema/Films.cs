using Capy.Domain.Models.Auth;

namespace Capy.Domain.Models.Cinema
{
    public class Films
    {
        public Guid Id { get; set; }
        public long? KinopoiskId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string PosterUrl { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
