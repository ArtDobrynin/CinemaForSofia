namespace Capy.Common.Interfaces
{
    public interface IAuthService
    {
        string GenerateSecurityToken(Guid id, string email, string name);
    }
}
