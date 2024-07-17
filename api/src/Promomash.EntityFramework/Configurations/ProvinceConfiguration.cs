namespace Promomash.EntityFramework.Configurations;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("Province", "dbo");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(250);

        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(250);
    }
}