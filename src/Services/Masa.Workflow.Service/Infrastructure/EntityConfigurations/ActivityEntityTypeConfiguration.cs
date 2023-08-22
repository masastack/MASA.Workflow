namespace Masa.Workflow.Service.Infrastructure.EntityConfigurations;

public class ActivityEntityTypeConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Type).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Description).HasMaxLength(4000);
        builder.OwnsOne(
            a => a.Point, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        builder.OwnsOne(
            a => a.Wires, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        builder.OwnsOne(
            a => a.Meta, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        builder.OwnsOne(
            a => a.RetryPolicy, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.Property(p => p.FirstRetryInterval).HasConversion<string>();
                ownedNavigationBuilder.Property(p => p.RetryTimeout).HasConversion<string>();
                ownedNavigationBuilder.Property(p => p.MaxRetryInterval).HasConversion<string>();
            });
        builder.OwnsOne(
            a => a.InputLabels, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });

        builder.OwnsOne(
            a => a.OutputLabels, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
    }
}
