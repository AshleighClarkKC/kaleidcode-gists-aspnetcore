using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaleidocode.Gists.ChangeTracking.Models.Auditing;
using Kaleidocode.Gists.ChangeTracking.Models.Base;
using Kaleidocode.Gists.ChangeTracking.Repositories.Contracts;
using Kaleidocode.Gists.ChangeTracking.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Kaleidocode.Gists.ChangeTracking.Repositories.Base;

public abstract class BaseRepository<TEntity, TContext>(TContext context) : IBaseRepository<TEntity>
    where TEntity : BaseModel, new()
    where TContext : DbContext
{
    private TContext Context { get; init; } = context;

    public virtual void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        TrackChanges(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
        TrackChanges(entity);
    }

    public virtual void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
        TrackChanges(entity);
    }

    private void TrackChanges(TEntity entity)
    {
        // Unsure how necessary this call is, but will be removed as the behaviour is understood. 
        Context.ChangeTracker.DetectChanges();

        foreach (EntityEntry<TEntity> ce in Context.ChangeTracker.Entries<TEntity>())
        {
            if (ce.IsTracked())
            {
                foreach (var prop in ce.Properties)
                {
                    var auditEntry = new AuditEntryModel
                    {
                        EntryId = entity.Id,
                        Actor = DetermineActor(entity, ce.State),
                        EntityName = ce.Entity.GetType().Name,
                        State = Enum.GetName(ce.State),
                        AffectedProperty = prop.Metadata.Name,
                        OriginalValue = prop.OriginalValue?.ToString(),
                        NewValue = prop.CurrentValue?.ToString(),
                        ValueCreated = ce.Entity.Created,
                        ValueUpdated = ce.Entity.Updated,
                        IsValueActive = ce.Entity.Active,
                        IsValueDeleted = ce.Entity.Deleted,
                        EntryCreated = DateTime.UtcNow
                    };

                    Context.Set<AuditEntryModel>().Add(auditEntry);
                }
            }
        }
    }

    private static Guid? DetermineActor(TEntity entity, EntityState state)
    {
        Guid? actorId = null;

        switch (state)
        {
            case EntityState.Added:
                {
                    actorId = entity.CreatedBy;
                    break;
                }
            case EntityState.Modified:
                {
                    actorId = entity.UpdatedBy;
                    break;
                }
            case EntityState.Deleted:
                {
                    actorId = entity.DeletedBy;
                    break;
                }
            default:
                break;
        }

        return actorId;
    }
}

