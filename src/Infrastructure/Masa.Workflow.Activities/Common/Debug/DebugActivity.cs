using Masa.Workflow.Activities.Contracts;
using Masa.Workflow.Activities.Contracts.Debug;
using Masa.Workflow.Core.Hubs;
using Masa.Workflow.Interactive.Runner;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Masa.Workflow.Activities.Debug;

public class DebugActivity : MasaWorkflowActivity<DebugMeta>
{
    private readonly IHubContext<DebugHub, IDebugHub> _hubContext;
    private readonly ILogger<DebugActivity> _logger;

    public DebugActivity(IHubContext<DebugHub, IDebugHub> hubContext, ILogger<DebugActivity> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public override async Task<ActivityExecutionResult> RunAsync(DebugMeta input, Message msg)
    {
        var logObj = await GetLogObjectAsync(msg, input.Property);
        
        _logger.Log(LogLevel.Information, "DebugActivity log: {0}", JsonSerializer.Serialize(logObj));

        var debugResult = new DebugResult()
        {
            DebugAt = DateTimeOffset.UtcNow,
            Log = JsonSerializer.Serialize(logObj)
        };

        if (input.LogToDebugWindow)
        {
            await _hubContext.Clients.Group(WorkflowId.ToString()).Log(debugResult);
        }

        return await base.RunAsync(input, msg);
    }

    public async Task<object?> GetLogObjectAsync(Message message, string? property)
    {
        Console.Out.WriteLine("property = {0}", property);
        
        if (string.IsNullOrWhiteSpace(property))
        {
            return message;
        }

        if (string.Equals(property, "Payload", StringComparison.OrdinalIgnoreCase))
        {
            return message.Payload;
        }

        var runner = new CSharpRunner(typeof(Message).Assembly);
        var code = $"return msg.{property};";
        return await runner.RunWithMessageAsync<object>(code, message);
    }
}
