using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly ILogger<CpuMetricsController> _logger;


        public CpuMetricsController(
            ILogger<CpuMetricsController> logger,
            ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _logger = logger;
        }


        //[HttpPost("create")]
        //public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        //{
        //    CpuMetricDto cpuMetric = new CpuMetricDto
        //    {
        //        Time = request.Time,
        //        Value = request.Value
        //    };

        //    int result = _cpuMetricsRepository.Create(cpuMetric);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", cpuMetric);

        //    return Ok(result);
        //}

        //[HttpDelete("delete")]
        //public IActionResult Delete([FromQuery] int id)
        //{
        //    int result = _cpuMetricsRepository.Delete(id);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", id);

        //    return Ok(result);
        //}

        //[HttpPatch("update")]
        //public IActionResult Update([FromBody] CpuMetricDto cpuMetric)
        //{
        //    int result = _cpuMetricsRepository.Update(cpuMetric);

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно обновили cpu метрику: {0}", cpuMetric);

        //    return Ok(result);
        //}

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _cpuMetricsRepository.GetAll();
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(metric);
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули метрик: {0}", response.Metrics.Count);

            return Ok(response);
        }

        //[HttpGet("byid")]
        //public IActionResult GetMetricById([FromQuery] int id)
        //{
        //    var metric = _cpuMetricsRepository.GetById(id);
        //    var response = new AllCpuMetricsResponse()
        //    {
        //        Metrics = new List<CpuMetricDto>()
        //    };
        //    if (metric != null)
        //    {
        //        response.Metrics.Add(metric);
        //    }

        //    if (_logger != null)
        //        _logger.LogDebug("Успешно вернули метрику: {0}", id);

        //    return Ok(response);
        //}

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            if (_logger != null)
                _logger.LogDebug($"Успешно вернули метрику с: {fromTime} по: {toTime}");
            return Ok(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }
    }
}
