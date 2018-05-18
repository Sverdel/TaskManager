using Microsoft.IdentityModel.Tokens;

namespace TaskManager.Api.Models.Configs
{
    public interface IAccountConfig
    {
        string Issuer { get; }
        SymmetricSecurityKey Key { get; }
        int LifetimeMinutes { get; }
        string SiteUrl { get; }
    }
}