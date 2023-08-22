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

    public async Task CreateAsync()
    {
        //todo create
        var orderEvent = new WorkflowCreatedDomainEvent();
        await EventBus.PublishAsync(orderEvent);
    }

    public async Task<IList<Flow>> QueryListAsync()
    {
        return await _workflowRepository.GetPaginatedListAsync(1, 20);
    }
}