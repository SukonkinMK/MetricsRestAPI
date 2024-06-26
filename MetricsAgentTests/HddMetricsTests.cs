using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MetricsAgentTests
{
    public class HddMetricsTests
    {
        private readonly HddMetricsController _controller;
        private readonly Mock<IHddMetricsRepository> repositoryMock;
        private readonly Mock<ILogger<HddMetricsController>> loggerMock;

        public HddMetricsTests()
        {
            repositoryMock = new Mock<IHddMetricsRepository>();
            loggerMock = new Mock<ILogger<HddMetricsController>>();
            _controller = new HddMetricsController(loggerMock.Object, repositoryMock.Object);
        }

        [Fact]
        public void Get_ShouldCall_Get_From_Repository()
        {
            repositoryMock.Setup(repositoryMock => repositoryMock.GetByTimePeriod(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).Returns(new List<HddMetric>() { new HddMetric() });

            var result = _controller.GetMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10)) as OkObjectResult;

            Assert.NotNull(result.Value);
            var list = result.Value as List<HddMetric>;
            Assert.Single(list);
        }

        [Fact]
        public void GetHddMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(10);

            IActionResult result = _controller.GetMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
