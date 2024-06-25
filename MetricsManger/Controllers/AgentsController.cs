using MetricsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private IAgentPool<AgentInfo> _agentPool;
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(IAgentPool<AgentInfo> agentPool, ILogger<AgentsController> logger)
        {
            _agentPool = agentPool;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            if (agentInfo != null)
            {
                _agentPool.Add(agentInfo);
            }
            if (_logger != null)
                _logger.LogDebug($"Успешно добавили нового агента: id:{agentInfo.AgentId} adr:{agentInfo.AgentAddress} enabled:{agentInfo.Enable}") ;
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Values.ContainsKey(agentId))
            {
                _agentPool.Values[agentId].Enable = true;
                if (_logger != null)
                    _logger.LogDebug($"Успешно изменили статус агента id: {agentId}");
            }
            else
            {
                if (_logger != null)
                    _logger.LogDebug($"Агент id: {agentId} не найден");
            }
            return Ok();
        }


        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Values.ContainsKey(agentId))
            {
                _agentPool.Values[agentId].Enable = false;
                if (_logger != null)
                    _logger.LogDebug($"Успешно изменили статус агента id: {agentId}");
            }
            else
            {
                if (_logger != null)
                    _logger.LogDebug($"Агент id: {agentId} не найден");
            }
            return Ok();
        }

        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            if (_logger != null)
                _logger.LogDebug($"Успешно получен список агентов из {_agentPool.Get().Length} шт.");
            return Ok(_agentPool.Get());
        }

    }
}
