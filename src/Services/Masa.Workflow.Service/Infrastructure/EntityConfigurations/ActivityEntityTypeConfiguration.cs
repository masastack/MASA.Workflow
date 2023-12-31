﻿namespace Masa.Workflow.Service.Infrastructure.EntityConfigurations;

public class ActivityEntityTypeConfiguration : IEntityTypeConfiguration<Masa.Workflow.Service.Domain.Aggregates.Activity>
{
    public void Configure(EntityTypeBuilder<Masa.Workflow.Service.Domain.Aggregates.Activity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Type).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Description).HasMaxLength(4000);

        builder.Property(a => a.Wires).HasJsonConversion();
        builder.Property(a => a.Meta).HasJsonConversion();

        builder.OwnsOne(
            a => a.RetryPolicy, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.Property(p => p.FirstRetryInterval).HasConversion<string>();
                ownedNavigationBuilder.Property(p => p.RetryTimeout).HasConversion<string>();
                ownedNavigationBuilder.Property(p => p.MaxRetryInterval).HasConversion<string>();
            });
    }
}
