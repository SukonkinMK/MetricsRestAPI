using Azure;
using MetricsData;
using MetricsData.Requests;
using MetricsManager.Models;
using MetricsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MetricsManager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly IAgetInfoRepository _agentPool;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, IMetricsAgentClient metricsAgentClient, IAgetInfoRepository agentPool)
        {
            _logger = logger;
            _agentPool = agentPool;
            _metricsAgentClient = metricsAgentClient;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            MetricsResponse<CpuMetricDto> response = _metricsAgentClient.GetCpuMetrics(new MetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            if (_logger != null)
                _logger.LogDebug($"Выполнен запрос метрик CPU для агента {agentId} с {fromTime} по {toTime}");
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            if (_logger != null)
                _logger.LogDebug($"Успешно получена метрика CPU всех агентов с {fromTime} по {toTime}");
            return Ok();
        }

    }
}
