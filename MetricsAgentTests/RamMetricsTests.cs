using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgentTests
{
    public class RamMetricsTests
    {
        private readonly RamMetricsController _controller;

        private readonly Mock<IRamMetricsRepository> repositoryMock;
        private readonly Mock<ILogger<RamMetricsController>> loggerMock;

        public RamMetricsTests()
        {
            repositoryMock = new Mock<IRamMetricsRepository>();
            loggerMock = new Mock<ILogger<RamMetricsController>>();
            _controller = new RamMetricsController(loggerMock.Object, repositoryMock.Object);
        }

        [Fact]
        public void Get_ShouldCall_Get_From_Repository()
        {
            repositoryMock.Setup(repositoryMock => repositoryMock.GetAll()).Returns(new List<RamMetric>() { new RamMetric() });

            var result = _controller.GetMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10)) as OkObjectResult;

            Assert.NotNull(result.Value);
            var list = result.Value as List<RamMetric>;
            Assert.Single(list);
        }

        [Fact]
        public void GetRamMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(10);

            IActionResult result = _controller.GetMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
