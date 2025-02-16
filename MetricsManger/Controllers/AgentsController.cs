using MetricsManager.Models;
using MetricsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private IAgetInfoRepository _agentPool;
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(IAgetInfoRepository agentPool, ILogger<AgentsController> logger)
        {
            _agentPool = agentPool;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfoDto agentInfo)
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
            AgentInfoDto agentInfo = new AgentInfoDto() { AgentId = agentId, Enable = true };
            int res = _agentPool.Update(agentInfo);
            if (res != -1)
            {
                if (_logger != null)
                    _logger.LogDebug($"Успешно изменили статус агента id: {agentId}");
                return Ok(res);
            }
            else
            {
                if (_logger != null)
                    _logger.LogDebug($"Агент id: {agentId} не найден");
                return BadRequest($"Агент id: {agentId} не найден");
            }
        }


        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            AgentInfoDto agentInfo = new AgentInfoDto() {AgentId = agentId, Enable = false };
            int res = _agentPool.Update(agentInfo);
            if (res != -1)
            {                
                if (_logger != null)
                    _logger.LogDebug($"Успешно изменили статус агента id: {agentId}");
                return Ok(res);
            }
            else
            {
                if (_logger != null)
                    _logger.LogDebug($"Агент id: {agentId} не найден");
                return BadRequest($"Агент id: {agentId} не найден");
            }            
        }

        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            var response = _agentPool.Get();
            if (_logger != null)
                _logger.LogDebug($"Успешно получен список агентов из {response.Count} шт.");
            return Ok(response);
        }

    }
}
