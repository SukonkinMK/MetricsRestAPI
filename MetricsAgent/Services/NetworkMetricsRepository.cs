using AutoMapper;
using MetricsAgent.Models;

namespace MetricsAgent.Services
{
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private readonly MetricsContext _context;
        private readonly IMapper _mapper;

        public NetworkMetricsRepository(MetricsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(NetworkMetricDto item)
        {
            using (_context)
            {
                var entity = _mapper.Map<NetworkMetric>(item);
                _context.NetworkMetrics.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }

        public int Delete(int id)
        {
            using (_context)
            {
                var entity = _context.NetworkMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    _context.NetworkMetrics.Remove(entity);
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public int Update(NetworkMetricDto item)
        {
            using (_context)
            {
                var entity = _context.NetworkMetrics.FirstOrDefault(x => x.Id == item.Id);
                if (entity != null)
                {
                    entity.Update(_mapper.Map<NetworkMetric>(item));
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public IList<NetworkMetricDto> GetAll()
        {
            using (_context)
            {
                var returnList = _context.NetworkMetrics.Select(x => _mapper.Map<NetworkMetricDto>(x)).ToList();
                return returnList;
            }
        }
        public NetworkMetricDto GetById(int id)
        {
            using (_context)
            {
                var entity = _context.NetworkMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    return _mapper.Map<NetworkMetricDto>(entity);
                }
                return null;
            }
        }

        public IList<NetworkMetricDto> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (_context)
            {
                var list = _context.NetworkMetrics.Where(x => x.Time > fromTime.TotalSeconds && x.Time < toTime.TotalSeconds).ToList();
                return list.Select(x => _mapper.Map<NetworkMetricDto>(x)).ToList();
            }
        }
    }
}
