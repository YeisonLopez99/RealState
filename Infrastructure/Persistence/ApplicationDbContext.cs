using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Owner> Owners { get; set; } = default!;
    public DbSet<Property> Properties { get; set; } = default!;
    public DbSet<PropertyImage> PropertyImages { get; set; } = default!;
    public DbSet<PropertyTrace> PropertyTraces { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Owner>(b =>
        {
            b.HasKey(x => x.IdOwner);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Email).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Property>(b =>
        {
            b.HasKey(x => x.IdProperty);           
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
            b.HasIndex(x => x.CodeExternal);
        });
        modelBuilder.Entity<Property>()
            .HasMany(p => p.Images)
            .WithOne(pi => pi.Property)
            .HasForeignKey(pi => pi.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
             .HasMany(p => p.Traces)
             .WithOne(pi => pi.Property)
             .HasForeignKey(pi => pi.PropertyIdProperty)
             .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Property>()
            .Navigation(p => p.Images)
            .HasField("_images");

        modelBuilder.Entity<Property>()
            .Navigation(p => p.Traces)
            .HasField("_traces");

        modelBuilder.Entity<PropertyImage>(b =>
        {
            b.HasKey(x => x.IdPropertyImage);
            b.Property(x => x.FileName).HasMaxLength(250);
            b.Property(x => x.Base64).HasColumnType("nvarchar(max)");
        });

        modelBuilder.Entity<PropertyTrace>(b =>
        {
            b.HasKey(x => x.IdPropertyTrace);
            b.Property(x => x.Value).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<RefreshToken>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.TokenHash).IsRequired();
        });
    }
}