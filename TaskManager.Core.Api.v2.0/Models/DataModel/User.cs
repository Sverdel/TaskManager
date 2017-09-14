using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace TaskManager.Core.Api.Models.DataModel
{
    public class User : IdentityUser
    {
        [NotMapped]
        public string Token { get; set; }

        //[NotMapped]
        //public string ExternalId { get; private set; }

        //[NotMapped]
        //public string LoginProvider { get; private set; }

        //public static User FromIdentity(ClaimsIdentity identity)
        //{
        //    Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        //    //if (!(providerKeyClaim != null
        //    //    && !string.IsNullOrEmpty(providerKeyClaim.Issuer)
        //    //    && !string.IsNullOrEmpty(providerKeyClaim.Value)
        //    //    && providerKeyClaim.Issuer != ClaimsIdentity.DefaultIssuer))
        //    //{
        //    //    throw new Ar
        //    //}
        //    //, "authorization failed", HttpStatusCode.BadRequest);

        //    return new User
        //    {
        //        UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
        //        Email = identity.FindFirst(ClaimTypes.Email).Value,
        //        LoginProvider = providerKeyClaim.Issuer,
        //        ExternalId = providerKeyClaim.Value
        //    };
        //}
    }
}