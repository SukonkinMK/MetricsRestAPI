using AutoMapper;
using MetricsData;
using MetricsManager.Models;

namespace MetricsManager.Services
{
    public class AgetInfoRepository : IAgetInfoRepository
    {
        private readonly AgetsContext _context;
        private readonly IMapper _mapper;

        public AgetInfoRepository(AgetsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Add(AgentInfoDto value)
        {
            using (_context)
            {
                //AgentInfo entity = _mapper.Map<AgentInfo>(value);
                AgentInfo entity = new AgentInfo() { AgentAddress = value.AgentAddress.ToString(), Enable = value.Enable};
                _context.AgentInfos.Add(entity);
                _context.SaveChanges();
                return entity.AgentId;
            }
        }

        public IList<AgentInfoDto> Get()
        {
            using (_context)
            {
                var returnList = _context.AgentInfos.Select(x => _mapper.Map<AgentInfoDto>(x)).ToList();
                return returnList;
            }
        }

        public int Update(AgentInfoDto value)
        {
            using (_context)
            {
                var entity = _context.AgentInfos.FirstOrDefault(x =>  x.AgentId == value.AgentId);
                if (entity != null)
                {
                    entity.Enable = value.Enable;
                    _context.SaveChanges();
                    return entity.AgentId;
                }
                else
                    return -1;
            }
        }
    }
}
