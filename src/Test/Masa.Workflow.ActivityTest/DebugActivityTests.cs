using System.Dynamic;
using Masa.Workflow.Activities.Debug;
using Masa.Workflow.Core;
using Masa.Workflow.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Masa.Workflow.ActivityTest;

public class DebugActivityTests
{
    [Fact]
    public async Task GetLogObject_PropertyNull_EqualMessage()
    {
        var services = new ServiceCollection();
        services.AddSignalR();
        var serviceProvider = services.BuildServiceProvider();
        var hubContext = serviceProvider.GetService<IHubContext<DebugHub, IDebugHub>>();
        var logger = serviceProvider.GetService<ILogger<DebugActivity>>();
        // todo: mock the hubcontext

        var debugActivity = new DebugActivity(hubContext, logger);
        var message = new Message();

        var logObject = await debugActivity.GetLogObjectAsync(message, null);

        Assert.Equal(message, logObject);
    }

    [Fact]
    public async Task GetLogObject_PropertyPayload_EqualPayload()
    {
        var services = new ServiceCollection();
        services.AddSignalR();
        var serviceProvider = services.BuildServiceProvider();
        var hubContext = serviceProvider.GetService<IHubContext<DebugHub, IDebugHub>>();
        var logger = serviceProvider.GetService<ILogger<DebugActivity>>();
        // todo: mock the hubcontext

        var debugActivity = new DebugActivity(hubContext, logger);
        var message = new Message();
        message.Payload = Guid.NewGuid();

        var logObject = await debugActivity.GetLogObjectAsync(message, "Payload");
        Assert.Equal(message.Payload, logObject);
    }

    [Fact]
    public async Task GetLogObject_PropertyCustom_EqualCustom()
    {
        var services = new ServiceCollection();
        services.AddSignalR();
        var serviceProvider = services.BuildServiceProvider();
        var hubContext = serviceProvider.GetService<IHubContext<DebugHub, IDebugHub>>();
        var logger = serviceProvider.GetService<ILogger<DebugActivity>>();
        // todo: mock the hubcontext

        var debugActivity = new DebugActivity(hubContext, logger);
        dynamic message = new Message();
        message.CustomProperty = Guid.NewGuid();

        var logObject = await debugActivity.GetLogObjectAsync(message, "CustomProperty");
        Assert.Equal(message.CustomProperty, logObject);
    }

    [Fact]
    public async Task GetLogObject_PropertyDotDotProperty_EqualDotDotProperty()
    {
        var services = new ServiceCollection();
        services.AddSignalR();
        var serviceProvider = services.BuildServiceProvider();
        var hubContext = serviceProvider.GetService<IHubContext<DebugHub, IDebugHub>>();
        var logger = serviceProvider.GetService<ILogger<DebugActivity>>();
        // todo: mock the hubcontext

        var debugActivity = new DebugActivity(hubContext, logger);
        dynamic message = new Message();

        message.CustomProperty = new ExpandoObject();
        message.CustomProperty.NestProperty = Guid.NewGuid();

        var logObject = await debugActivity.GetLogObjectAsync(message, "CustomProperty.NestProperty");
        Assert.Equal(message.CustomProperty.NestProperty, logObject);
    }
}
