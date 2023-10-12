namespace Masa.Workflow.Service.Infrastructure.Repositories;

public class WorkflowRepository : Repository<WorkflowDbContext, Flow, Guid>, IWorkflowRepository
{
    public WorkflowRepository(WorkflowDbContext context, IUnitOfWork unitOfWork)
        : base(context, unitOfWork)
    {
    }

    public async Task<Flow> GetAsync(Guid id, params string[] includeProperties)
    {
        var query = Context.Set<Flow>().AsQueryable();
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query.FirstOrDefaultAsync(x => x.Id == id) ?? throw new UserFriendlyException("workflow id not find");
    }
}