﻿using System;

namespace TaskManager.Core.Api.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string AccessToken { get; set; }

        public DateTime TokenExpireDate { get; set; }
    }
}