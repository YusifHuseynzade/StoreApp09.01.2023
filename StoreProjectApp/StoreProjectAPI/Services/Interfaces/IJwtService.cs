using Store.Core.Entities;

namespace StoreProjectAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user, IList<string> roles, IConfiguration confg);
    }
}
