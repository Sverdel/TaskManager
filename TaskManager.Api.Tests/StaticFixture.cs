using AutoMapper;
using TaskManager.Api.Models;

namespace TaskManager.Api.Tests
{
    public class StaticFixture
    {
        public StaticFixture()
        {
            Mapper.Initialize(m => m.AddProfile<MappingProfile>());
        }
    }
}
