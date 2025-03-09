namespace Capy.Common.Contracts.LoginUser
{
    public class LoginUserResponse
    {
        public LoginUserItem Data { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class LoginUserItem
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
