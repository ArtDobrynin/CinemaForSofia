using System;
using System.Collections.Generic;
namespace CapyAuth.Application.Models.Auth.Registration
{
    public class RegisterUserResult
    {
        public string? Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ErrorMessage { get; set; }
    }
}
