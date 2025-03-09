namespace CapyFilms.Application.Models.Cinema.RandomFilm
{
    public class RandomFilmResult
    {
        public RandomFilmItem Data { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class RandomFilmItem
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string PosterUrl { get; set; }
    }
}
