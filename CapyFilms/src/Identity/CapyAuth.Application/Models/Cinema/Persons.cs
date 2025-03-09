using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CapyFilms.Application.Models.People
{
    public class PersonByNameResponseItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class PersonByNameResponse
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("docs")]
        public List<PersonByNameResponseItem> Items { get; set; }
    }
}