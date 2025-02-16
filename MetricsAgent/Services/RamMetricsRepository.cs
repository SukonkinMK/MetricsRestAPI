using AutoMapper;
using MetricsAgent.Models;
using MetricsData;

namespace MetricsAgent.Services
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly MetricsContext _context;
        private readonly IMapper _mapper;

        public RamMetricsRepository(MetricsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(RamMetricDto item)
        {
            using (_context)
            {
                var entity = _mapper.Map<RamMetric>(item);
                _context.RamMetrics.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }

        public int Delete(int id)
        {
            using (_context)
            {
                var entity = _context.RamMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    _context.RamMetrics.Remove(entity);
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public int Update(RamMetricDto item)
        {
            using (_context)
            {
                var entity = _context.RamMetrics.FirstOrDefault(x => x.Id == item.Id);
                if (entity != null)
                {
                    entity.Update(_mapper.Map<RamMetric>(item));
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public IList<RamMetricDto> GetAll()
        {
            using (_context)
            {
                var returnList = _context.RamMetrics.Select(x => _mapper.Map<RamMetricDto>(x)).ToList();
                return returnList;
            }
        }
        public RamMetricDto GetById(int id)
        {
            using (_context)
            {
                var entity = _context.RamMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    return _mapper.Map<RamMetricDto>(entity);
                }
                return null;
            }
        }

        public IList<RamMetricDto> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (_context)
            {
                var list = _context.RamMetrics.Where(x => x.Time > fromTime.TotalSeconds && x.Time < toTime.TotalSeconds).ToList();
                return list.Select(x => _mapper.Map<RamMetricDto>(x)).ToList();
            }
        }
    }
}
