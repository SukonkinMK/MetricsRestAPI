using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
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

        //[HttpPost("create")]
        //public IActionResult Create([FromBody] RamMetricDto metric)
        //{
        //    int result = _ramMetricsRepository.Create(metric);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", result);

        //    return Ok(result);
        //}

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _ramMetricsRepository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(metric);
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули метрик: {0}", response.Metrics.Count);

            return Ok(response);
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
