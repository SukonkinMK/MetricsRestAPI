using MetricsManager.Controllers;
using MetricsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions.Ordering;

namespace MetricsManagerTests
{

    [Order(1)]
    public class AgentsControllerTests
    {
        private readonly AgentsController _agentsController;
        private readonly AgentPool _agentPool;
        private readonly Mock<ILogger<AgentsController>> loggerMock;


        public AgentsControllerTests()
        {
            _agentPool = LazyAgentPool.Instance;
            loggerMock = new Mock<ILogger<AgentsController>>();
            _agentsController = new AgentsController(_agentPool, loggerMock.Object);
        }

        [Fact, Order(1)]
        public void RegisterAgentTest()
        {
            int agentId = 1;
            AgentInfo agentInfo = new AgentInfo() { AgentId = agentId, Enable = true };
            IActionResult actionResult = _agentsController.RegisterAgent(agentInfo);
            Assert.IsAssignableFrom<IActionResult>(actionResult);
        }

        [Fact, Order(2)]
        public void GetAgentsTest()
        {
            IActionResult actionResult = _agentsController.GetAllAgents();
            OkObjectResult result = Assert.IsAssignableFrom<OkObjectResult>(actionResult);

            //(IEnumerable<AgentInfo>)result.Value 
            //result.Value as IEnumerable<AgentInfo>
            Assert.NotNull(result.Value as IEnumerable<AgentInfo>);
            Assert.NotEmpty((IEnumerable<AgentInfo>)result.Value);
        }

        [Fact, Order(3)]
        public void DisableAgentByIdTest()
        {
            int agentId = 1;
            AgentInfo? agentBefore = ((_agentsController.GetAllAgents() as OkObjectResult)?.Value as IEnumerable<AgentInfo>)?.FirstOrDefault(item => item.AgentId == agentId);
            if (agentBefore != null)
            {
                _agentsController.DisableAgentById(agentId);
                AgentInfo? agentAfter = ((_agentsController.GetAllAgents() as OkObjectResult)?.Value as IEnumerable<AgentInfo>)?.FirstOrDefault(item => item.AgentId == agentId);
                if (agentAfter != null)
                {
                    Assert.False(agentAfter.Enable);
                }
            }
            else
                Assert.Fail($"Agent {agentId} not found");
        }

        [Fact, Order(4)]
        public void EnableAgentByIdTest()
        {
            int agentId = 1;
            AgentInfo? agentBefore = ((_agentsController.GetAllAgents() as OkObjectResult)?.Value as IEnumerable<AgentInfo>)?.FirstOrDefault(item => item.AgentId == agentId);
            if (agentBefore != null)
            {
                _agentsController.EnableAgentById(agentId);
                AgentInfo? agentAfter = ((_agentsController.GetAllAgents() as OkObjectResult)?.Value as IEnumerable<AgentInfo>)?.FirstOrDefault(item => item.AgentId == agentId);
                if (agentAfter != null)
                {
                    Assert.True(agentAfter.Enable);
                }
            }
            else
                Assert.Fail($"Agent {agentId} not found");
        }

    }
}
