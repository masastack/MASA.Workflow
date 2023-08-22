namespace Masa.Workflow.Service.Infrastructure.Repositories;

public class WorkflowRepository : Repository<WorkflowDbContext, Flow>, IWorkflowRepository
{
    public WorkflowRepository(WorkflowDbContext context, IUnitOfWork unitOfWork)
        : base(context, unitOfWork)
    {
    }
}