namespace Masa.Workflow.Service.Infrastructure.EntityConfigurations;

public class FlowVersionEntityTypeConfiguration : IEntityTypeConfiguration<FlowVersion>
{
    public void Configure(EntityTypeBuilder<FlowVersion> builder)
    {
        builder.Property(b => b.VersionNumber).HasMaxLength(20);
        builder.OwnsOne(
            f => f.Activities, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
    }
}
