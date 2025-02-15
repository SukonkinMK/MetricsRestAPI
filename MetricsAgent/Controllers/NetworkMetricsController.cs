using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly INetworkMetricsRepository _networkMetricsRepository;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository networkMetricsRepository)
        {
            _logger = logger;
            _networkMetricsRepository = networkMetricsRepository;
        }

        //[HttpPost("create")]
        //public IActionResult Create([FromBody] NetworkMetricDto metric)
        //{
        //    int result = _networkMetricsRepository.Create(metric);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", result);

        //    return Ok(result);
        //}

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _networkMetricsRepository.GetByTimePeriod(fromTime, toTime);
            if (_logger != null)
                _logger.LogDebug("Успешно вернули Network метрику: __");
            return Ok(metrics);
        }
    }
}
