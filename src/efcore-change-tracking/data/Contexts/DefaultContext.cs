
using Kaleidocode.Gists.ChangeTracking.Models.Base;
using Kaleidocode.Gists.ChangeTracking.Models.Auditing;
using Microsoft.EntityFrameworkCore;

namespace Kaleidocode.Gists.ChangeTracking.Data.Contexts;

public class DefaultContext(DbContextOptions<DefaultContext> contextOptions) : DbContext(contextOptions)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ChangeTracker.AutoDetectChangesEnabled = true;

        modelBuilder
            .Entity<AuditEntryModel>()
            .ToTable(
                name: "Entry",
                schema: "Audit"
            );

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<AuditEntryModel> AuditEntries { get; set; }
}

