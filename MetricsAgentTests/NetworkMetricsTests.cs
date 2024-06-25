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
    public class NetworkMetricsTests
    {
        private readonly NetworkMetricsController _controller;
        private readonly Mock<INetworkMetricsRepository> repositoryMock;
        private readonly Mock<ILogger<NetworkMetricsController>> loggerMock;

        public NetworkMetricsTests()
        {
            repositoryMock = new Mock<INetworkMetricsRepository>();
            loggerMock = new Mock<ILogger<NetworkMetricsController>>();
            _controller = new NetworkMetricsController(loggerMock.Object, repositoryMock.Object);
        }

        [Fact]
        public void Get_ShouldCall_Get_From_Repository()
        {
            repositoryMock.Setup(repositoryMock => repositoryMock.GetAll()).Returns(new List<NetworkMetric>() { new NetworkMetric() });

            var result = _controller.GetMetrics(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10)) as OkObjectResult;

            Assert.NotNull(result.Value);
            var list = result.Value as List<NetworkMetric>;
            Assert.Single(list);
        }

        [Fact]
        public void GetNetworkMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(10);

            IActionResult result = _controller.GetMetrics(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
