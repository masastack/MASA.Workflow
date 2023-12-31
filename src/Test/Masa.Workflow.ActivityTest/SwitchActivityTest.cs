﻿using Masa.Workflow.Activities;
using Masa.Workflow.Activities.Contracts.Switch;
using Masa.Workflow.Activities.Switch;
using Masa.Workflow.Core;

namespace Masa.Workflow.ActivityTest
{
    public class SwitchActivityTest
    {
        ServiceCollection _services;
        WorkflowHub _hub;

        public SwitchActivityTest()
        {
            _hub = new Mock<WorkflowHub>().Object;
            _services = new ServiceCollection();
            _services.AddMasaWorkflow();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData("adg256")]
        public async void Test(object payload)
        {
            var msg = new Message();
            msg.Payload = payload;
            var _switchActivity = new SwitchActivity(_services.BuildServiceProvider().GetRequiredService<IRulesEngineClient>());
            var meta = new SwitchMeta()
            {
                Rules = new List<Rule>
                    {
                        new Rule(Operator.Eq, "1"),
                        new Rule(Operator.Lt, "2"),
                        new Rule(Operator.True),
                        new Rule(Operator.NotNull),
                        new Rule(Operator.Empty),
                        new Rule(Operator.NotEmpty),
                        new Rule(Operator.Otherwise),
                        new Rule(Operator.IsType, "string"),
                        new Rule(Operator.Contains, "2")
                    },
            };
            var result = await _switchActivity.RunAsync(meta, null);
            Assert.NotNull(result);
            //Assert.True(result.Count > 0);
        }
    }
}
