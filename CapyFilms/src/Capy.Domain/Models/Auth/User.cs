using Capy.Domain.Models.Cinema;

namespace Capy.Domain.Models.Auth
{
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Хэш пароля пользователя
        /// </summary>
        public string HashPassword { get; set; }

        public List<Films> Films { get; set; } = new List<Films>();
    }
}
