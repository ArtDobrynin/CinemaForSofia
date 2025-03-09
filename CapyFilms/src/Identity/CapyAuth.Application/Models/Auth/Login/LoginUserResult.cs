namespace CapyAuth.Application.Models.Auth.Login
{
    public class LoginUserResult
    {
        public string? Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MessageError { get; set; }
    }
}
