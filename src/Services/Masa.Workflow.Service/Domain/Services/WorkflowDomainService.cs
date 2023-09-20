namespace Masa.Workflow.Service.Domain.Services;

public class WorkflowDomainService : DomainService
{
    private readonly ILogger<WorkflowDomainService> _logger;
    private readonly IWorkflowRepository _workflowRepository;

    public WorkflowDomainService(IDomainEventBus eventBus, ILogger<WorkflowDomainService> logger, IWorkflowRepository workflowRepository) : base(eventBus)
    {
        _logger = logger;
        _workflowRepository = workflowRepository;
    }

    public async Task SaveAsync()
    {

    }

    public async Task StartAsync()
    {

    }

    public async Task<IList<Flow>> QueryListAsync()
    {
        return await _workflowRepository.GetPaginatedListAsync(1, 20);
    }

    public async Task DeleteAsync(Guid workflowId)
    {
        var entity = await _workflowRepository.FindAsync(w => w.Id == workflowId);
        if (entity == null)
        {
            _logger.LogError($"The Id {workflowId} does not exist");
            throw new UserFriendlyException("The Id does not exist");
        }
        //todo chech run
        await _workflowRepository.RemoveAsync(entity);
    }
}