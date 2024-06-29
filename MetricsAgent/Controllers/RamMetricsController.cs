using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _ramMetricsRepository;

        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository ramMetricsRepository)
        {
            _logger = logger;
            _ramMetricsRepository = ramMetricsRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricDto metric)
        {
            int result = _ramMetricsRepository.Create(metric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", result);

            return Ok(result);
        }

        [HttpGet("available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _ramMetricsRepository.GetByTimePeriod(fromTime, toTime);
            if (_logger != null)
                _logger.LogDebug($"Успешно вернули метрику RAM с: {fromTime} по: {toTime}");
            return Ok(metrics);
        }
    }
}
