using CatApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatApi.Services.Database;


public class CatDbContext : DbContext
{
    public DbSet<CatEntity> Cats { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<CatTagEntity> CatTag {get;set;}

    public CatDbContext(DbContextOptions<CatDbContext> options) : base(options) 
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CatEntity>()
            .HasIndex(c => c.CatId)
            .IsUnique(true);

        modelBuilder.Entity<TagEntity>()
            .HasIndex(c => c.Name)
            .IsUnique(true);
            
        // modelBuilder.Entity<CatEntity>()
        //     .HasMany(c => c.Tags)
        //     .WithMany(t => t.Cats)
        //     .UsingEntity<Dictionary<string, object>>(
        //         "CatTag",
        //         j => j.HasOne<TagEntity>().WithMany().HasForeignKey("TagId"),
        //         j => j.HasOne<CatEntity>().WithMany().HasForeignKey("CatId"));

        modelBuilder.Entity<CatTagEntity>()
            .HasKey(ct => new { ct.CatId, ct.TagId });

        modelBuilder.Entity<CatTagEntity>()
            .HasOne(ct => ct.Cat)
            .WithMany(c => c.CatTags)
            .HasForeignKey(ct => ct.CatId);

        modelBuilder.Entity<CatTagEntity>()
            .HasOne(ct => ct.Tag)
            .WithMany(t => t.CatTags)
            .HasForeignKey(ct => ct.TagId);




    }
}