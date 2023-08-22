namespace Masa.Workflow.Service.Infrastructure;

public class WorkflowDbContext : MasaDbContext
{
    public WorkflowDbContext(MasaDbContextOptions<WorkflowDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    protected override void OnModelCreatingExecuting(ModelBuilder builder)
    {
        builder.HasDefaultSchema("wf");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()!);
        base.OnModelCreatingExecuting(builder);
    }
}