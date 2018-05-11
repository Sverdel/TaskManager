using System;

namespace TaskManager.Api.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public virtual string UserName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public string Provider { get; set; }

        public string AccessToken { get; set; }

        public DateTime TokenExpireDate { get; set; }

        public string PhoneNumber { get; set; }
    }
}
