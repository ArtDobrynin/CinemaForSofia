using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace CapyFilms.Application.Models.Cinema.GetNewFilms
{
    public class NewFilmsItem
    {
        public int total { get; set; }
        public int totalPages { get; set; }
        public IEnumerable<Film> items { get; set; }
    }

    public class Film
    {
        [JsonPropertyName("kinopoiskId")]
        public long? KinopoiskId { get; set; }
        [JsonPropertyName("imdbId")]
        public string? ImdbId { get; set; }
        [JsonPropertyName("nameRu")]
        public string? NameRu { get; set; }
        [JsonPropertyName("nameEn")]
        public string? NameEn { get; set; }
        [JsonPropertyName("nameOriginal")]
        public string? NameOriginal { get; set; }
        [JsonPropertyName("ratingKinopoisk")]
        public double? RatingKinopoisk { get; set; }
        [JsonPropertyName("ratingImdb")]
        public double? RatingImdb { get; set; }
        [JsonPropertyName("year")]
        public int? Year { get; set; }
        [JsonPropertyName("posterUrl")]
        public string? PosterUrl { get; set; }
    }

    public class MovieDtoV1_4
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
