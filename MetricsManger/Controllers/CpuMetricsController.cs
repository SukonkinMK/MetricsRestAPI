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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAgetInfoRepository _agentPool;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, IHttpClientFactory httpClientFactory, IAgetInfoRepository agentPool)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _agentPool = agentPool;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            AgentInfoDto agentInfo = _agentPool.Get().FirstOrDefault(a => a.AgentId == agentId);
            if (agentInfo == null)
            {
                if (_logger != null)
                    _logger.LogDebug($"Не удалось найти пользователя с id {agentId}");
                return BadRequest($"Agent id {agentId} not exists");
            }
            string requestQuery = $"{agentInfo.AgentAddress}api/metrics/cpu/from/{fromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{toTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = httpClient.SendAsync(httpRequestMessage).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseStr = responseMessage.Content.ReadAsStringAsync().Result;
                List<CpuMetricDto> metrics = (List<CpuMetricDto>)JsonConvert.DeserializeObject(responseStr, typeof(List<CpuMetricDto>));
                CpuMetricsResponse cpuMetricsResponse = new CpuMetricsResponse { Metrics = metrics, AgentId = agentId };
                if (_logger != null)
                    _logger.LogDebug($"Успешно полученo {metrics.Count} метрик CPU для агента {agentId} с {fromTime} по {toTime}");
                return Ok(cpuMetricsResponse);
            }
            else
            {
                if (_logger != null)
                    _logger.LogDebug($"Не удалось получить метрики для агента id: {agentId} с {fromTime} по {toTime}");
                return BadRequest();
            }
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
