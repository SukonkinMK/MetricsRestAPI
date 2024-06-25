using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
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
        public DotNetMetricsTests() 
        {
            _dotNetMetricsController = new DotNetMetricsController();
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
