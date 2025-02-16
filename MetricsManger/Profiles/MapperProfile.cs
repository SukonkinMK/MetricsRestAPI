using AutoMapper;
using MetricsData;
using MetricsManager.Models;

namespace MetricsManager.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AgentInfo, AgentInfoDto>().ReverseMap();  
        }
    }
}
