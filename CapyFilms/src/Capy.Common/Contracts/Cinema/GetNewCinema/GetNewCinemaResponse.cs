namespace Capy.Common.Contracts.Cinema.GetNewCinema
{
    public class GetNewCinemaResponse
    {
        public IEnumerable<GetNewFilmsItem> Data { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class GetRandomCinemaResponse
    {
        public GetNewFilmsItem Data { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class GetNewFilmsItem
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string PosterUrl { get; set; }
    }
}
