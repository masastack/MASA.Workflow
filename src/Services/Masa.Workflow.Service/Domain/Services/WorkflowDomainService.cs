namespace Masa.Workflow.Service.Domain.Services;
using Activity = Masa.Workflow.Service.Domain.Aggregates.Activity;

public class WorkflowDomainService : DomainService
{
    private readonly ILogger<WorkflowDomainService> _logger;
    private readonly IWorkflowRepository _workflowRepository;

    public WorkflowDomainService(IDomainEventBus eventBus, ILogger<WorkflowDomainService> logger, IWorkflowRepository workflowRepository)
        : base(eventBus)
    {
        _logger = logger;
        _workflowRepository = workflowRepository;
    }

    public async Task<Guid> SaveAsync(WorkflowSaveRequest workflowRequest)
    {
        Flow flow;
        if (workflowRequest.Id.IsNullOrEmpty())
        {
            flow = new Flow(workflowRequest.Name, workflowRequest.Description);
        }
        else
        {
            flow = await _workflowRepository.GetAsync(Guid.Parse(workflowRequest.Id),nameof(Flow.Activities),nameof(Flow.Versions))
                ?? throw new UserFriendlyException("workflow id not find");
            flow.UpdateInfo(workflowRequest.Name, workflowRequest.Description);
        }
        flow.AddVersion(workflowRequest.NodeJson);
        flow.SetActivities(ResolveJson(workflowRequest.NodeJson));

        if (flow.Id == Guid.Empty)
        {
            await _workflowRepository.AddAsync(flow);
        }
        else
        {
            await _workflowRepository.UpdateAsync(flow);
        }

        return flow.Id;

        List<Activity> ResolveJson(string nodeJson)
        {
            var result = new List<Activity>();

            using var document = JsonDocument.Parse(nodeJson);
            var root = document.RootElement;
            var nodes = root.GetProperty("drawflow").GetProperty("Home").GetProperty("data");

            foreach (var nodeObject in nodes.EnumerateObject())
            {
                var node = nodeObject.Value;
                var type = node.GetProperty("name").GetString();

                var data = JsonDocument.Parse(node.GetProperty("data").GetProperty("data").GetString()).RootElement;

                data.GetProperty("Id").TryGetGuid(out var id);

                var name = data.GetProperty("Name").GetString();
                var description = data.TryGetProperty("Description", out var descProperty) ? descProperty.GetString() : null;

                var outputs = node.GetProperty("outputs").EnumerateObject();
                Wires wires = new Wires();
                foreach (var output in outputs)
                {
                    var item = new List<Guid>();
                    var connections = output.Value.GetProperty("connections").EnumerateArray();
                    foreach (var connection in connections)
                    {
                        var nodeId = connection.GetProperty("node").GetString();

                        var nodeIdData = JsonDocument.Parse(nodes.GetProperty(nodeId).GetProperty("data").GetProperty("data").GetString()).RootElement;
                        var wireId = Guid.Parse(nodeIdData.GetProperty("Id").GetString());
                        item.Add(wireId);
                    }
                    wires.Add(item);
                }

                var metaData = JsonSerializer.Deserialize<MetaData>(data.GetProperty("Meta").GetString());
                result.Add(new Activity(id, name, type, description, wires, metaData));
            }
            return result;
        }
    }

    public async Task DeleteAsync(Guid workflowId)
    {
        var entity = await GetByIdAsync(workflowId);
        //todo check run
        await _workflowRepository.RemoveAsync(entity);
    }

    public async Task UpdateStatusAsync(Guid workflowId, WorkflowStatus status)
    {
        var entity = await GetByIdAsync(workflowId);
        entity.SetStatus(status);
        await _workflowRepository.UpdateAsync(entity);
    }

    private async Task<Flow> GetByIdAsync(Guid workflowId)
    {
        var entity = await _workflowRepository.FindAsync(w => w.Id == workflowId);
        if (entity == null)
        {
            _logger.LogError($"The Id {workflowId} does not exist");
            throw new UserFriendlyException("The Id does not exist");
        }
        return entity;
    }
}