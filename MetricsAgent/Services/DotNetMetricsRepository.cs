using AutoMapper;
using MetricsAgent.Models;
using MetricsData;


namespace MetricsAgent.Services
{
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly MetricsContext _context;
        private readonly IMapper _mapper;

        public DotNetMetricsRepository(MetricsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(DotNetMetricDto item)
        {
            using (_context)
            {
                var entity = _mapper.Map<DotNetMetric>(item);
                _context.DotNetMetrics.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }

        public int Delete(int id)
        {
            using (_context)
            {
                var entity = _context.DotNetMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    _context.DotNetMetrics.Remove(entity);
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public int Update(DotNetMetricDto item)
        {
            using (_context)
            {
                var entity = _context.DotNetMetrics.FirstOrDefault(x => x.Id == item.Id);
                if (entity != null)
                {
                    entity.Update(_mapper.Map<DotNetMetric>(item));
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public IList<DotNetMetricDto> GetAll()
        {
            using (_context)
            {
                var returnList = _context.DotNetMetrics.Select(x => _mapper.Map<DotNetMetricDto>(x)).ToList();
                return returnList;
            }
        }
        public DotNetMetricDto GetById(int id)
        {
            using (_context)
            {
                var entity = _context.DotNetMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    return _mapper.Map<DotNetMetricDto>(entity);
                }
                return null;
            }
        }

        public IList<DotNetMetricDto> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (_context)
            {
                var list = _context.DotNetMetrics.Where(x => x.Time > fromTime.TotalSeconds && x.Time < toTime.TotalSeconds).ToList();
                return list.Select(x => _mapper.Map<DotNetMetricDto>(x)).ToList();
            }
        }
    }
}
