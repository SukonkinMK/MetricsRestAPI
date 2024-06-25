using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
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


        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            CpuMetric cpuMetric = new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            };

            _cpuMetricsRepository.Create(cpuMetric);

            if (_logger != null)
                _logger.LogDebug("Успешно добавили новую cpu метрику: {0}", cpuMetric);

            return Ok();
        }

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
                response.Metrics.Add(new CpuMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
                _logger.LogDebug("Успешно вернули метрик: {0}", response.Metrics.Count);

            return Ok(response);
        }


        [HttpGet("sql-test")]
        public IActionResult TryToSqlLite()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";
            using (var con = new SQLiteConnection(cs))
            {
                con.Open();
                using var cmd = new SQLiteCommand(stm, con);
                string version = cmd.ExecuteScalar().ToString();
                return Ok(version);

            }
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {

            if (_logger != null)
                _logger.LogDebug($"Успешно вернули метрику с: {fromTime} по: {toTime}");
            return Ok();
        }
    }
}
