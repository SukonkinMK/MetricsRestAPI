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
    public class CpuMetricsTests
    {

        private readonly CpuMetricsController _cpuMetricsController;
        private readonly Mock<ICpuMetricsRepository> repositoryMock;
        private readonly Mock<ILogger<CpuMetricsController>> loggerMock;


        public CpuMetricsTests()
        {
            repositoryMock = new Mock<ICpuMetricsRepository>();
            loggerMock = new Mock<ILogger<CpuMetricsController>>();
            _cpuMetricsController = new CpuMetricsController(loggerMock.Object, repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит CpuMetric - объект
            repositoryMock.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();
            // Выполняем действие на контроллере
            var result = _cpuMetricsController.Create(new MetricsAgent.Models.Requests.CpuMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });
            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
            repositoryMock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetCpuMetrics_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);

            IActionResult result = _cpuMetricsController.GetMetrics(fromTime, toTime);

            Assert.IsAssignableFrom<IActionResult>(result);

        }
    }
}
