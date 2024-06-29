using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _dotnetMetricsRepository;
        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository)
        {
            _logger = logger;
            _dotnetMetricsRepository = repository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricDto metric)
        {

            int result = _dotnetMetricsRepository.Create(metric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", result);

            return Ok(result);
        }

        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetErrorsCount([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _dotnetMetricsRepository.GetByTimePeriod(fromTime, toTime);
            if (_logger != null)
                _logger.LogDebug("Успешно вернули DotNet метрику: __");
            return Ok(metrics);
        }

    }
}
