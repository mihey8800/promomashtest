namespace Promomash.EntityFramework.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Country", "dbo");

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