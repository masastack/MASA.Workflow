namespace Masa.Workflow.Service.Infrastructure.EntityConfigurations;

public class FlowVersionEntityTypeConfiguration : IEntityTypeConfiguration<FlowVersion>
{
    public void Configure(EntityTypeBuilder<FlowVersion> builder)
    {
        builder.Property(b => b.VersionNumber).HasMaxLength(20);
        builder.Property(b => b.Json).HasComment("workflow definition json");
    }
}
