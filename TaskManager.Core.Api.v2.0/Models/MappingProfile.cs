using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.Core.Api.Models.Dto;

namespace TaskManager.Core.Api.Models
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<WorkTask, TaskDto>().ReverseMap();
        }
    }
}
