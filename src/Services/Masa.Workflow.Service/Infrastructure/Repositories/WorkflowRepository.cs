namespace Masa.Workflow.Service.Infrastructure.Repositories;

public class WorkflowRepository : Repository<WorkflowDbContext, Flow, Guid>, IWorkflowRepository
{
    public WorkflowRepository(WorkflowDbContext context, IUnitOfWork unitOfWork)
        : base(context, unitOfWork)
    {
    }
}