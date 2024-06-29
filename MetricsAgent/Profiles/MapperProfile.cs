using AutoMapper;
using MetricsAgent.Models;

namespace MetricsAgent.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>().ForMember(x => x.Time, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
            CreateMap<CpuMetricDto, CpuMetric>().ForMember(x => x.Time, opt => opt.MapFrom(src => src.Time.TotalSeconds));

            CreateMap<DotNetMetric, DotNetMetricDto>().ForMember(x => x.Time, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
            CreateMap<DotNetMetricDto, DotNetMetric>().ForMember(x => x.Time, opt => opt.MapFrom(src => src.Time.TotalSeconds));

            CreateMap<HddMetric, HddMetricDto>().ForMember(x => x.Time, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
            CreateMap<HddMetricDto, HddMetric>().ForMember(x => x.Time, opt => opt.MapFrom(src => src.Time.TotalSeconds));

            CreateMap<NetworkMetric, NetworkMetricDto>().ForMember(x => x.Time, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
            CreateMap<NetworkMetricDto, NetworkMetric>().ForMember(x => x.Time, opt => opt.MapFrom(src => src.Time.TotalSeconds));

            CreateMap<RamMetric, RamMetricDto>().ForMember(x => x.Time, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));
            CreateMap<RamMetricDto, RamMetric>().ForMember(x => x.Time, opt => opt.MapFrom(src => src.Time.TotalSeconds));
        }        
    }
}
