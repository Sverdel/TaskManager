using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Core.Api.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string AccessToken { get; set; }

        public DateTime TokenExpireDate { get; set; }
    }
}
