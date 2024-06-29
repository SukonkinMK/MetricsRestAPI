using AutoMapper;
using MetricsAgent.Models;

namespace MetricsAgent.Services
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {        
        private readonly MetricsContext _context;
        private readonly IMapper _mapper;

        public CpuMetricsRepository(MetricsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(CpuMetricDto item)
        {
            using (_context)
            {
                var entity = _mapper.Map<CpuMetric>(item);
                _context.CpuMetrics.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }

        public int Delete(int id)
        {
            using (_context)
            {
                var entity = _context.CpuMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {
                    _context.CpuMetrics.Remove(entity);
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public int Update(CpuMetricDto item)
        {
            using(_context)
            {
                var entity = _context.CpuMetrics.FirstOrDefault(x => x.Id == item.Id);
                if (entity != null)
                {
                    entity.Update(_mapper.Map<CpuMetric>(item));
                    _context.SaveChanges();
                    return entity.Id;
                }
                return 0;
            }
        }

        public IList<CpuMetricDto> GetAll()
        {
            using (_context)
            {
                var returnList = _context.CpuMetrics.Select(x => _mapper.Map<CpuMetricDto>(x)).ToList();
                return returnList;
            }
        }
        public CpuMetricDto GetById(int id)
        {
            using (_context)
            {
                var entity = _context.CpuMetrics.FirstOrDefault(x => x.Id.Equals(id));
                if (entity != null)
                {                    
                    return _mapper.Map<CpuMetricDto>(entity);
                }
                return null;
            }
        }

        public IList<CpuMetricDto> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (_context)
            {
                var list = _context.CpuMetrics.Where(x => x.Time > fromTime.TotalSeconds && x.Time < toTime.TotalSeconds).ToList();
                return list.Select(x => _mapper.Map<CpuMetricDto>(x)).ToList();
            }
        }
    }
}
