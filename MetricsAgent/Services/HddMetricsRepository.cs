using AutoMapper;
using MetricsAgent.Models;
using MetricsData;

namespace MetricsAgent.Services
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly MetricsContext _context;
        private readonly IMapper _mapper;

        public HddMetricsRepository(MetricsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(HddMetricDto item)
        {
            using (_context)
            {
                var entity = _mapper.Map<HddMetric>(item);
                _context.HddMetrics.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }

        public int Delete(int id)
        {
            using (_context)
            {
                var entity = _context.HddMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    _context.HddMetrics.Remove(entity);
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public int Update(HddMetricDto item)
        {
            using (_context)
            {
                var entity = _context.HddMetrics.FirstOrDefault(x => x.Id == item.Id);
                if (entity != null)
                {
                    entity.Update(_mapper.Map<HddMetric>(item));
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public IList<HddMetricDto> GetAll()
        {
            using (_context)
            {
                var returnList = _context.HddMetrics.Select(x => _mapper.Map<HddMetricDto>(x)).ToList();
                return returnList;
            }
        }
        public HddMetricDto GetById(int id)
        {
            using (_context)
            {
                var entity = _context.HddMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    return _mapper.Map<HddMetricDto>(entity);
                }
                return null;
            }
        }

        public IList<HddMetricDto> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (_context)
            {
                var list = _context.HddMetrics.Where(x => x.Time > fromTime.TotalSeconds && x.Time < toTime.TotalSeconds).ToList();
                return list.Select(x => _mapper.Map<HddMetricDto>(x)).ToList();
            }
        }
    }
}
