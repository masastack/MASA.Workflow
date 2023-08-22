namespace Masa.Workflow.Service.Infrastructure.EntityConfigurations;

public class FlowEntityTypeConfiguration : IEntityTypeConfiguration<Flow>
{
    public void Configure(EntityTypeBuilder<Flow> builder)
    {
        builder.ToTable("Workflow");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(50).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(4000);
        builder.HasMany(b => b.Activities).WithOne(a => a.Flow).HasForeignKey(a => a.FlowId);
        builder.HasMany(b => b.Versions).WithOne(a => a.Flow).HasForeignKey(a => a.FlowId);

        builder.OwnsOne(
            a => a.EnvironmentVariables, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
    }
}
