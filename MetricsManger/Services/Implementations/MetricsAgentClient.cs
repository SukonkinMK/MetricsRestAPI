using MetricsData;
using MetricsData.Requests;
using MetricsManager.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace MetricsManager.Services.Implementations
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly  HttpClient _httpClient;
        private readonly IAgetInfoRepository _agentPool;
        private readonly ILogger<MetricsAgentClient> _logger;

        public MetricsAgentClient(HttpClient httpClient, IAgetInfoRepository repository, ILogger<MetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _agentPool = repository;
            _logger = logger;
        }

        public MetricsResponse<CpuMetricDto> GetCpuMetrics(MetricsRequest cpuMetricsRequest)
        {
            AgentInfoDto agentInfo = _agentPool.Get().FirstOrDefault(a => a.AgentId == cpuMetricsRequest.AgentId);
            if (agentInfo == null)
            {
                if (_logger != null)
                    _logger.LogDebug($"Не удалось найти пользователя с id {cpuMetricsRequest.AgentId}");
                throw new Exception($"Agent id {cpuMetricsRequest.AgentId} not exists");
            }
            string requestQuery = $"{agentInfo.AgentAddress}api/metrics/cpu/from/{cpuMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{cpuMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpResponseMessage responseMessage = _httpClient.SendAsync(httpRequestMessage).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseStr = responseMessage.Content.ReadAsStringAsync().Result;
                List<CpuMetricDto> metrics = (List<CpuMetricDto>)JsonConvert.DeserializeObject(responseStr, typeof(List<CpuMetricDto>));
                MetricsResponse<CpuMetricDto> cpuMetricsResponse = new MetricsResponse<CpuMetricDto> { Metrics = metrics, AgentId = cpuMetricsRequest.AgentId };
                if (_logger != null)
                    _logger.LogDebug($"Успешно полученo {metrics.Count} метрик CPU для агента {cpuMetricsRequest.AgentId} с {cpuMetricsRequest.FromTime} по {cpuMetricsRequest.ToTime}");
                return cpuMetricsResponse;
            }
            else
            {
                if (_logger != null)
                    _logger.LogDebug($"Не удалось получить метрики для агента id: {cpuMetricsRequest.AgentId} с {cpuMetricsRequest.FromTime} по {cpuMetricsRequest.ToTime}");
                return null;
            }
        }

        public MetricsResponse<HddMetricDto> GetHddMetrics(MetricsRequest request)
        {
            throw new NotImplementedException();
        }

        public MetricsResponse<NetworkMetricDto> GetNetworkMetrics(MetricsRequest request)
        {
            throw new NotImplementedException();
        }

        public MetricsResponse<RamMetricDto> GetRamMetrics(MetricsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
