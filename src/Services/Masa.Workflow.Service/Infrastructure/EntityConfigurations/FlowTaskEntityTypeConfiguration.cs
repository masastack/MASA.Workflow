
namespace Masa.Workflow.Service.Infrastructure.EntityConfigurations;

public class FlowTaskEntityTypeConfiguration : IEntityTypeConfiguration<FlowTask>
{
    public void Configure(EntityTypeBuilder<FlowTask> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.WorkerHost).HasMaxLength(100);
        builder.Property(x => x.Message);
    }
}
