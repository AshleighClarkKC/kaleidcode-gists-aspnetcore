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

    #region Add

    public virtual void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        TrackChanges(entity);
        Context.SaveChanges();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Add(entity);
        TrackChanges(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region AddRange

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
        TrackChangesInCollection(entities);
        Context.SaveChanges();
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().AddRange(entities);
        TrackChangesInCollection(entities);
        await Context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Update

    public virtual void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
        TrackChanges(entity);
        Context.SaveChanges();
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Update(entity);
        TrackChanges(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region UpdateRange

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().UpdateRange(entities);
        TrackChangesInCollection(entities);
        Context.SaveChanges();
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().UpdateRange(entities);
        TrackChangesInCollection(entities);
        await Context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Delete

    public virtual void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
        TrackChanges(entity);
        Context.SaveChanges();
    }

    public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Remove(entity);
        TrackChanges(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region DeleteRange

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
        TrackChangesInCollection(entities);
        Context.SaveChanges();
    }

    public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().RemoveRange(entities);
        TrackChangesInCollection(entities);
        await Context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    private void TrackChangesInCollection(IEnumerable<TEntity> entities)
    {
        foreach (TEntity entity in entities) 
        {
            // Ignore IDE0059: This is due to the inherent behaviour of foreach loops. Sometimes they aren't referenced properly.
            var entityInstance = entity;
            TrackChanges(entity);
        }
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

