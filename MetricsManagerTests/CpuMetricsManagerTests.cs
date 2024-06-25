using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerTests
{
    public class CpuMetricsManagerTests
    {
        private CpuMetricsController _cpuMetricsController;
        private readonly Mock<ILogger<CpuMetricsController>> loggerMock;

        public CpuMetricsManagerTests()
        {
            loggerMock = new Mock<ILogger<CpuMetricsController>>();
            _cpuMetricsController = new CpuMetricsController(loggerMock.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnOk()
        {
            int agentId = 1;
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);

            IActionResult result = _cpuMetricsController.GetMetricsFromAgent(agentId, fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);


        }
        [Fact]
        public void GetMetricsFromAllCluster_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);

            IActionResult result = _cpuMetricsController.GetMetricsFromAllCluster( fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);


        }
    }
}
