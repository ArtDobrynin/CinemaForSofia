namespace Capy.Common.Contracts.RegisterUser
{
    public class RegisterUserResponse
    {
        public RegisterUserItem Data { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class RegisterUserItem
    {
        public string Token { get; set; }   
        public DateTime CreatedAt { get; set; }
    }
}
