using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManager.Api.Models.Configs
{
    public class AccountConfig : IAccountConfig
    {
        public AccountConfig(IConfiguration config)
        {
            Issuer = config["AppSettings:Issuer"];
            SiteUrl = config["AppSettings:SiteUrl"];
            LifetimeMinutes = int.Parse(config["AppSettings:Lifetime"]);
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:SecurityKey"]));
        }

        public string Issuer { get; }

        public string SiteUrl { get; }

        public int LifetimeMinutes { get; }

        public SymmetricSecurityKey Key { get;  }
    }
}
