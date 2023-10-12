namespace Masa.Workflow.Service.Domain.Repositories;


public interface IWorkflowRepository : IRepository<Flow, Guid>
{
    Task<Flow> GetAsync(Guid id, params string[] includeProperties);
}