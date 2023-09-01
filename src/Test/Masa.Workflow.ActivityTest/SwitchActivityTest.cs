using Masa.Workflow.ActivityCore;

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
            _services.AddDepend();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData("adg256")]
        public async void Test(object payload)
        {
            var msg = _services.BuildServiceProvider().GetRequiredService<Msg>();
            msg.Payload = payload;
            var _switchActivity = new SwitchActivity(_hub, msg, _services.BuildServiceProvider().GetRequiredService<IRulesEngineClient>());
            var input = new SwitchMeta()
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
                        new Rule(Operator.IsType,"string"),
                        new Rule(Operator.Contains,"2")
                    },
                Wires = new List<List<Guid>>
                    {
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() },
                        new List<Guid> { Guid.NewGuid() }
                    }
            };
            var result = await _switchActivity.RunAsync(input);
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
