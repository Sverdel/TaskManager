using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Core.Api.Models.DataModel
{
    public class User : IdentityUser
    {
        [NotMapped]
        public string Token { get; set; }
    }
}