namespace Masa.Workflow.ActivityTest
{
    public class SwitchActivityTest
    {
        SwitchActivity _switchActivity;

        public SwitchActivityTest()
        {
            var hub = new Mock<WorkflowHub>();
            var services = new ServiceCollection();
            services.AddDepend();
            _switchActivity = new SwitchActivity(hub.Object, services.BuildServiceProvider().GetRequiredService<IRulesEngineClient>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(true)]
        [InlineData("adg256")]
        public async void Test(object payload)
        {
            var input = new Msg<SwitchMeta>()
            {
                Payload = payload,
                ActivityId = Guid.NewGuid(),
                Meta = new SwitchMeta()
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
                }
            };
            var result = await _switchActivity.RunAsync(input);
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
