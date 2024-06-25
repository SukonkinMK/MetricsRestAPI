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
    public class DotNetMetricsTests
    {
        private readonly DotNetMetricsController _dotNetMetricsController;
        private readonly Mock<IDotNetMetricsRepository> repositoryMock;
        private readonly Mock<ILogger<DotNetMetricsController>> loggerMock;
        public DotNetMetricsTests()
        {
            repositoryMock = new Mock<IDotNetMetricsRepository>();
            loggerMock = new Mock<ILogger<DotNetMetricsController>>();
            _dotNetMetricsController = new DotNetMetricsController(loggerMock.Object, repositoryMock.Object);
        }

        [Fact]
        public void Get_ShouldCall_Get_From_Repository()
        {
            repositoryMock.Setup(repositoryMock => repositoryMock.GetAll()).Returns(new List<DotNetMetric>() { new DotNetMetric()});

            var result = _dotNetMetricsController.GetErrorsCount(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10)) as OkObjectResult;

            Assert.NotNull(result.Value);
            var list = result.Value as List<DotNetMetric>;
            Assert.Single(list);
        }

            [Fact]
        public void GetDotNetMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            IActionResult result = _dotNetMetricsController.GetErrorsCount(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
