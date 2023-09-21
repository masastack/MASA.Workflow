namespace Masa.Workflow.Service.Domain.Services;

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

    public async Task<Guid> SaveAsync(WorkflowRequest workflowRequest)
    {
        //todd workflowRequest.NodeJson convert Activity
        return Guid.Empty;
    }

    public async Task DeleteAsync(Guid workflowId)
    {
        var entity = await _workflowRepository.FindAsync(w => w.Id == workflowId);
        if (entity == null)
        {
            _logger.LogError($"The Id {workflowId} does not exist");
            throw new UserFriendlyException("The Id does not exist");
        }
        //todo check run
        await _workflowRepository.RemoveAsync(entity);
    }
}