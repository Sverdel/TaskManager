using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Core.Api.Models.DataModel
{
    public class User : IdentityUser
    {
        [NotMapped]
        public string Token { get; set; }
    }
}